using Croco.Core.Abstractions.Application;
using CrocoLanding.Configuration.Hangfire;
using CrocoLanding.Configuration.Swagger;
using CrocoLanding.CrocoStuff;
using CrocoShop.CrocoStuff;
using CrocoShop.Model.Contexts;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CrocoLanding
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Croco = new StartupCroco(new StartUpCrocoOptions
            {
                Configuration = configuration,
                Env = env,
                ApplicationActions = new List<Action<ICrocoApplication>>
                {
                    ApplicationServiceRegistrator.Register
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
            services.AddControllersWithViews()
                .AddJsonOptions(options => ConfigureJsonSerializer(options.JsonSerializerOptions));

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString(LandingDbContext.ConnectionString)));

            SwaggerConfiguration.ConfigureSwagger(services, new List<string>
            {
            });

            //Установка приложения
            //Croco.SetCrocoApplication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseFileServer();;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            HangfireConfiguration.AddHangfire(app, false);
        }
    }
}