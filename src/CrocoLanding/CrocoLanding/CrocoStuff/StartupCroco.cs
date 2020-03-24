using Croco.Core.Abstractions.Application;
using Croco.Core.Application;
using Croco.Core.Application.Options;
using Croco.Core.Common.Enumerations;
using Croco.Core.EventSourcing.Implementations.StatusLog;
using Croco.Core.Extensions;
using Croco.Core.Extensions.Implementations;
using Croco.Core.Hangfire.Extensions;
using Croco.Core.Logic.Models.Files;
using Croco.Core.Model.Entities.Store;
using Croco.WebApplication.Application;
using CrocoLanding.Implementations;
using CrocoLanding.Logic;
using CrocoLanding.Model.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Zoo.Core;

namespace CrocoLanding.CrocoStuff
{
    public class StartupCroco
    {
        IConfiguration Configuration { get; }
        IWebHostEnvironment Env { get; }

        List<Action<ICrocoApplication>> ApplicationActions { get; }
        List<Action<ICrocoApplicationOptions>> BuildActions { get; }
        List<Action<IServiceCollection>> ServiceRegistrations { get; }
        string ApplicationUrl { get; }

        public StartupCroco(StartUpCrocoOptions options)
        {
            Configuration = options.Configuration;
            Env = options.Env;
            ApplicationActions = options.ApplicationActions;
            BuildActions = options.BuildActions;
            ServiceRegistrations = options.ServiceRegistrations;
            ApplicationUrl = options.ApplicationUrl;
        }

        static readonly List<LoggedApplicationAction> Logs = new Lazy<List<LoggedApplicationAction>>().Value;

        static readonly object LogsLocker = new object();

        static void LogAction(LoggedApplicationAction log)
        {
            lock (LogsLocker)
            {
                Logs.Add(log);

                if (Logs.Count == 10)
                {
                    CrocoApp.Application.EventSourcer.Publisher
                        .PublishAsync(SystemCrocoExtensions.GetRequestContext(), Logs).GetAwaiter().GetResult();

                    Logs.Clear();
                }
            }
        }

        public void RegisterCrocoApplication(IServiceCollection services)
        {
            //Регистрация сервисов
            ServiceRegistrations.ForEach(x => x(services));

            var memCache = new MemoryCache(new MemoryCacheOptions());

            services.AddSingleton<IMemoryCache, MemoryCache>(s => memCache);

            var baseOptions = new EFCrocoApplicationOptions
            {
                CacheManager = new ApplicationCacheManager(memCache),
                GetDbContext = () => LandingDbContext.Create(Configuration),
                RequestContextLogger = new CrocoWebAppRequestContextLogger(),
                FileOptions = new CrocoFileOptions
                {
                    SourceDirectory = Env.WebRootPath,
                    ImgFileResizeSettings = new List<ImgFileResizeSetting>
                    {
                        new ImgFileResizeSetting
                        {
                            ImageSizeName = ImageSizeType.Icon.ToString(),
                            MaxHeight = 50,
                            MaxWidth = 50
                        },

                        new ImgFileResizeSetting
                        {
                            ImageSizeName = ImageSizeType.Small.ToString(),
                            MaxHeight = 200,
                            MaxWidth = 200
                        },

                        new ImgFileResizeSetting
                        {
                            ImageSizeName = ImageSizeType.Medium.ToString(),
                            MaxHeight = 500,
                            MaxWidth = 500
                        }
                    },
                },
                RootPath = Env.ContentRootPath,
            }.GetApplicationOptions();

            baseOptions.StateHandler = new DatabaseCrocoMessageStateHandler(baseOptions.DateTimeProvider);

            baseOptions
                .AddHangfireEventSourcerAndJobManager(new CrocoServiceRegistrator(services))
                .AddDelayedApplicationLogger(LogAction);

            foreach(var buildAction in BuildActions)
            {
                buildAction(baseOptions);
            }

            var options = new CrocoWebApplicationOptions()
            {
                ApplicationUrl = ApplicationUrl,
                CrocoOptions = baseOptions,
            };

            var application = new LandingWebApplication(options) 
            {
                IsDevelopment = Env.EnvironmentName == "Development"
            };

            services.AddSingleton<ICrocoApplication>(application);
        }

        public void SetCrocoActivatorAndApplication(IServiceProvider serviceProvider)
        {
            CrocoApp.Activator = new CrocoActivator(serviceProvider);

            var app = serviceProvider.GetService<ICrocoApplication>();

            //Некоторые действия требуют уже установленного глобального приложения
            //Поэтому сначала устанавливаем глобальное приложение
            CrocoApp.Application = app;

            ApplicationActions?.ForEach(x => x(app));
        }
    }
}