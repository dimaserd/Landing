using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;

namespace CrocoLanding.Configuration.Hangfire
{
    public class HangfireConfiguration
    {
        public static void AddHangfire(IApplicationBuilder app, bool isFreeAccess = false)
        {
            var filters = isFreeAccess ? new IDashboardAuthorizationFilter[]
            {
                new FreeDashboardAuthorizationFilter()
            } :
            new IDashboardAuthorizationFilter[]
            {
                new MyDashboardAuthorizationFilter()
            };

            // Configure hangfire to use the new JobActivator we defined.
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(app.ApplicationServices));

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = filters
            });
            app.UseHangfireServer();
        }
    }
}