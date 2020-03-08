using Croco.Core.Abstractions.Application;
using Ecc.Implementation.TaskGivers;
using Hangfire;

namespace Ecc.Logic
{
    public static class EccServiceRegistrator
    {
        public static void AddJobs(ICrocoApplication application)
        {
            application.JobManager.AddJob<SendEmailTaskGiverByCallBackRequest>("some-id", Cron.Minutely(), srv => srv.GetTask());
            application.JobManager.AddJob<SendEmailTaskGiver>("SendEmailTaskGiver", Cron.Minutely(), srv => srv.GetTask());
        }
    }
}