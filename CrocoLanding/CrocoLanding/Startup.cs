using Croco.Core.Abstractions.Application;
using CrocoLanding.Configuration.Hangfire;
using CrocoLanding.Configuration.Swagger;
using CrocoLanding.CrocoStuff;
using CrocoLanding.Extensions;
using CrocoLanding.Implementations;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using CrocoLanding.Model.Entities.Clt.Default;
using Ecc.Implementation;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrocoLanding
{
    public class Startup
    {
        public const string DevelopmentEnvironmentName = "Development";

        const string SpaPath = "wwwroot";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Croco = new StartupCroco(new StartUpCrocoOptions
            {
                Configuration = configuration,
                Env = env,
                ApplicationActions = new List<Action<ICrocoApplication>>
                {
                    EccServiceRegistrator.AddJobs,
                },
            });
        }

        StartupCroco Croco { get; }

        IConfiguration Configuration { get; }

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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<ApplicationUserManager>();
            services.AddTransient<ApplicationSignInManager>();

            services.AddDbContext<LandingDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString(LandingDbContext.ConnectionString));
            });

            
            services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
            {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<LandingDbContext>()
            .AddDefaultTokenProviders();

            SwaggerConfiguration.ConfigureSwagger(services, new List<string>
            {
            });

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString(LandingDbContext.ConnectionString)));

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(5);
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });

            services.AddControllersWithViews().AddJsonOptions(options => ConfigureJsonSerializer(options.JsonSerializerOptions));
            services.AddRazorPages();

            services.AddSignalR().AddJsonProtocol(options => {
                ConfigureJsonSerializer(options.PayloadSerializerOptions);
            });

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            
            //Установка приложения
            Croco.RegisterCrocoApplication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            if (env.EnvironmentName != DevelopmentEnvironmentName)
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

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = SpaPath;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapAreaControllerRoute(
                    "admin",
                    "admin",
                    "Admin/{controller=Home}/{action=Index}/{id?}");

            });

            app.ConfigureExceptionHandler(new ApplicationLoggerManager());

            HangfireConfiguration.AddHangfire(app, env.EnvironmentName == DevelopmentEnvironmentName);
            Croco.SetCrocoActivatorAndApplication(app.ApplicationServices);
        }
    }
}