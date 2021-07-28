using Croco.Core.Application;
using Croco.Core.Common.Enumerations;
using Croco.Core.Contract.Application.Common;
using Croco.Core.Contract.Files;
using Croco.Core.EventSourcing;
using Croco.Core.Settings;
using Croco.WebApplication;
using Croco.WebApplication.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Croco.Common
{
    public class StartupCroco
    {
        string WebRootPath { get; }
        string ContentRootPath { get; }
        string ApplicationUrl { get; }
        string AppName { get; }
        public StartupCroco(StartUpCrocoOptions options)
        {
            WebRootPath = options.WebRootPath;
            ContentRootPath = options.ContentRootPath;
            ApplicationUrl = options.AppOptions.ApplicationUrl;
            AppName = options.AppOptions.ApplicationName;
        }

        public CrocoApplicationBuilder SetCrocoApplicationAndRegistratorAndGetBuilder<TDbContext>(IServiceCollection services)
            where TDbContext : DbContext
        {
            return CrocoWebApplicationRegistrator.RegisterWebApplication(services, new CrocoWebApplicationOptions
            {
                ApplicationName = AppName,
                ApplicationUrl = ApplicationUrl
            },
            appBuilder =>
            {
                EventSourcerBuilderExtensions
                .AddDatabaseCrocoMessageStateHandler<TDbContext>(appBuilder);

                SettingsBuilderExtensions
                    .AddDatabaseSettingFactory<TDbContext>(appBuilder);

                appBuilder.RegisterFileStorage(GetFileOptions());

                appBuilder.RegisterVirtualPathMapper(ContentRootPath);
            });
        }

        private CrocoFileOptions GetFileOptions()
        {
            return new CrocoFileOptions
            {
                SourceDirectory = WebRootPath,
                ImgFileResizeSettings = new Dictionary<string, ImgFileResizeSetting>
                {
                    [ImageSizeType.Icon.ToString()] = new ImgFileResizeSetting
                    {
                        MaxHeight = 50,
                        MaxWidth = 50
                    },
                    [ImageSizeType.Small.ToString()] = new ImgFileResizeSetting
                    {
                        MaxHeight = 200,
                        MaxWidth = 200
                    },
                    [ImageSizeType.Medium.ToString()] = new ImgFileResizeSetting
                    {
                        MaxHeight = 500,
                        MaxWidth = 500
                    }
                }
            };
        }
    }
}