using Hangfire.Dashboard;

namespace CrocoLanding.Configuration.Hangfire
{
    public class MyDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var user = httpContext.User;

            //Разрешаем всем пользователям с данными правами доступ к дашборду
            return false;
        }
    }
}