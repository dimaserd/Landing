using Clt.Logic.Extensions;
using Croco.Common;
using Croco.Core.Application;
using Croco.Core.Logic.DbContexts;
using Croco.WebApplication.Extensions;
using CrocoShop.Web.Logic.Services.Background;
using CrocoShop.Web.Extensions;
using CrocoShop.Web.Registrators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Zoo.DataImporter;
using Croco.Common.Registrators;
using Croco.Common.Options;
using Croco.Common.Services;
using CrocoShop.Web.Logic;

namespace CrocoShop.Web
{
    public class Startup
    {
        IConfiguration Configuration { get; }
        IWebHostEnvironment Environment { get; }
        StartupCrocoRegistrator CrocoStartUp { get; set; }
        CrocoApplicationBuilder Builder { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddControllersWithViews(opts => 
            {
                opts.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
            .AddControllersAsServices()
            .AddJsonOptions(options => ConfigureJsonSerializer(options.JsonSerializerOptions));
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.RegisterDbContexts(Configuration);

            // Prevent API from redirecting to login or Access Denied screens (Angular handles these).
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

            services.AddHostedService<DequeueIntegrationMessagesBackgroundService>();

            services
                .AddSignalR()
                .AddJsonProtocol(opts => ConfigureJsonSerializer(opts.PayloadSerializerOptions));

            var appOptions = Configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

            CrocoStartUp = new StartupCrocoRegistrator(new StartUpCrocoOptions
            {
                AppOptions = appOptions,
                ContentRootPath = Environment.ContentRootPath,
                WebRootPath = Environment.WebRootPath
            });

            Builder = CrocoStartUp.SetCrocoApplicationAndRegistratorAndGetBuilder<CrocoInternalDbContext>(services);
            ShopRegistrator.Register(Builder);
            DataImporterRegistrar.RegisterShopDataImporter(services);
            CourtRecordsRegistrator.RegisterServices(services);
        }

        private static void ConfigureJsonSerializer(JsonSerializerOptions settings)
        {
            settings.PropertyNameCaseInsensitive = true;
            settings.Converters.Add(new JsonStringEnumConverter());
        }

        private void LogData(ILogger logger)
        {
            logger.LogInformation($"Environment={Environment.EnvironmentName}");

            var dbOptions = Configuration.GetSection(nameof(DbConnectionOptions)).Get<DbConnectionOptions>();

            logger.LogInformation(JsonConvert.SerializeObject(dbOptions));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            DbCreator dbCreator,
            ILogger<CrocoApplication> logger)
        {
            LogData(logger);

            
            dbCreator.CreateDatabases();
            app.ConfigureExceptionHandler(logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            //before app.UseEndpoints
            app.Use(async (context, next) =>
            {
                HttpRequestExtensions.SettingRequestContextOnScope(context, ClaimsPrincipalExtensions.GetUserId);

                try
                {
                    await next.Invoke();
                }
                catch(Exception ex)
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
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            Builder.SetAppAndActivator(app.ApplicationServices);

            CourtSettingsRegistrator.RegisterCourtSettings(app);
        }
    }
}