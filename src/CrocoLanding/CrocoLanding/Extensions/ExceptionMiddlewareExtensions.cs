using Croco.Core.Application;
using Croco.Core.Extensions;
using CrocoLanding.Abstractions;
using Ecc.Contract.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace CrocoLanding.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var ex = contextFeature.Error;

                        await logger.LogExceptionAsync(ex);

                        await CrocoApp.Application.EventSourcer.Publisher.PublishAsync(SystemCrocoExtensions.GetRequestContext(), new ExceptionData
                        {
                            ExceptionName = ex.GetType().FullName,
                            ExceptionString = ex.ToString()
                        });
                    }
                });
            });
        }
    }
}