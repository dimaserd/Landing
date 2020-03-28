using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Application;
using Ecc.Implementation.Services;
using Ecc.Implementation.Settings;
using Ecc.Implementation.TaskGivers;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Core.Workers;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ecc.Implementation
{
    public static class EccServiceRegistrator
    {
        public static void AddJobs(ICrocoApplication application)
        {
            application.JobManager.AddJob<SendEmailTaskGiverByCallBackRequest>(nameof(SendEmailTaskGiverByCallBackRequest), Cron.Minutely(), srv => srv.GetTask());
            application.JobManager.AddJob<SendEmailTaskGiver>(nameof(SendEmailTaskGiver), Cron.Minutely(), srv => srv.GetTask());
        }

        public static void ConfigureServices(IServiceCollection services, string applicationUrl)
        {
            services.AddTransient<IEccPixelUrlProvider, AppEccPixelUrlProvider>(srv => new AppEccPixelUrlProvider(applicationUrl));
            services.AddTransient<IEmailSenderProvider, AppEmailSenderProvider>(srv =>
            {
                var setting = srv.GetService<ICrocoApplication>().SettingsFactory.GetSetting<SendGridEmailSettings>();

                return new AppEmailSenderProvider(setting);
            });

            services.AddTransient<IEccFileService, AppEccFileService>();
            services.AddTransient<IEccFilePathMapper, AppEccFilePathMapper>();
            services.AddTransient<IEccFileEmailsExtractor, AppEccEmailListExtractor>();
            services.AddTransient<IEccTextFunctionsProvider, MyAppEccTextFunctionsProvider > (srv => new MyAppEccTextFunctionsProvider($"{applicationUrl}/Api/Redirect/To?id={{0}}"));
            services.AddTransient(srv => new Func<ICrocoAmbientContext, EmailDelayedSender>(amb => new EmailDelayedSender(amb, srv.GetService<IEccPixelUrlProvider>(), srv.GetService<IEccTextFunctionsProvider>())));
        }
    }
}