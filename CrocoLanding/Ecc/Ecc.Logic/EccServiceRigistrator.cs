using Croco.Core.Abstractions.Application;
using Croco.Core.Extensions;
using Ecc.Logic.TaskGivers;
using Hangfire;

namespace Ecc.Logic
{
    public static class EccServiceRegistrator
    {
        public static void AddJobs(ICrocoApplication application)
        {
            application.AddJob<SendEmailTaskGiver>(Cron.Minutely());
        }
    }
}