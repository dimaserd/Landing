using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using System;
using System.Security.Claims;

namespace CrocoLanding.Configuration.Hangfire
{
    public class HangfireConfiguration
    {
        public static void AddHangfire(IApplicationBuilder app, Func<ClaimsPrincipal, bool> isAuthorizedFunc)
        {
            // Configure hangfire to use the new JobActivator we defined.
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(app.ApplicationServices));

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new IDashboardAuthorizationFilter[]
                {
                    new MyDashboardAuthorizationFilter(isAuthorizedFunc)
                }
            });
            app.UseHangfireServer();
        }
    }
}