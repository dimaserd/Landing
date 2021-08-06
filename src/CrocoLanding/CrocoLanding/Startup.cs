using Clt.Logic.Extensions;
using Croco.Common;
using Croco.Common.Options;
using Croco.Common.Services;
using Croco.Core.Application;
using Croco.Core.Logic.DbContexts;
using Croco.WebApplication.Extensions;
using CrocoLanding.Logic;
using CrocoLanding.Registrators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrocoLanding
{
    public class Startup
    {
        public const string DevelopmentEnvironmentName = "Development";

        const string SpaPath = "wwwroot";


        IConfiguration Configuration { get; }
        IWebHostEnvironment Environment { get; }
        StartupCroco CrocoStartUp { get; set; }
        CrocoApplicationBuilder Builder { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private static void ConfigureJsonSerializer(JsonSerializerOptions settings)
        {
            settings.PropertyNameCaseInsensitive = true;
            settings.PropertyNamingPolicy = null;
            settings.Converters.Add(new JsonStringEnumConverter());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = SpaPath;
            });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                options.ValueCountLimit = 200; // 200 items max
                options.ValueLengthLimit = 1024 * 1024 * 100; // 100MB max len form data
            });

            services.AddSwaggerGen(opts =>
            {
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });

            services.AddControllersWithViews()
                .AddControllersAsServices()
                .AddJsonOptions(options => ConfigureJsonSerializer(options.JsonSerializerOptions));

            services.AddRazorPages();

            services.AddSignalR().AddJsonProtocol(options => {
                ConfigureJsonSerializer(options.PayloadSerializerOptions);
            });

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            
            AppDbContextRegistrator.RegisterDbContexts(services, Configuration);
            

            var appOptions = Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

            CrocoStartUp = new StartupCroco(new StartUpCrocoOptions
            {
                AppOptions = appOptions,
                ContentRootPath = Environment.ContentRootPath,
                WebRootPath = Environment.WebRootPath
            });

            Builder = CrocoStartUp.SetCrocoApplicationAndRegistratorAndGetBuilder<CrocoInternalDbContext>(services);
            CrocoLandingLogicRegistrator.RegisterServices(Builder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DbCreator dbCreator)
        {
            dbCreator.CreateDatabases();
            app.UseDeveloperExceptionPage();

            if (!Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //before app.UseEndpoints
            app.Use(async (context, next) =>
            {
                HttpRequestExtensions.SettingRequestContextOnScope(context, ClaimsPrincipalExtensions.GetUserId);

                try
                {
                    await next.Invoke();
                }
                catch (Exception ex)
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

                    logger.LogError(ex, "Необработанная ошибка");

                    await context.Response.WriteAsJsonAsync(ex.Message);
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = SpaPath;
            });

            Builder.SetAppAndActivator(app.ApplicationServices);
        }
    }
}