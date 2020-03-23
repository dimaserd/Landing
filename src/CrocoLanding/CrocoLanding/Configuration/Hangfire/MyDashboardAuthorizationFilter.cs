using Hangfire.Dashboard;
using System;
using System.Security.Claims;

namespace CrocoLanding.Configuration.Hangfire
{
    public class MyDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public MyDashboardAuthorizationFilter(Func<ClaimsPrincipal, bool> isAuthFunc)
        {
            IsAuthFunc = isAuthFunc;
        }

        Func<ClaimsPrincipal, bool> IsAuthFunc { get; }

        public bool Authorize(DashboardContext context)
        {
            return IsAuthFunc(context.GetHttpContext().User);
        }
    }
}