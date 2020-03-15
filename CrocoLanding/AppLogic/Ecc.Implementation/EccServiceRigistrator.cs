using Croco.Core.Abstractions.Application;
using Ecc.Implementation.TaskGivers;
using Hangfire;

namespace Ecc.Implementation
{
    public static class EccServiceRegistrator
    {
        public static void AddJobs(ICrocoApplication application)
        {
            application.JobManager.AddJob<SendEmailTaskGiverByCallBackRequest>(nameof(SendEmailTaskGiverByCallBackRequest), Cron.Minutely(), srv => srv.GetTask());
            application.JobManager.AddJob<SendEmailTaskGiver>(nameof(SendEmailTaskGiver), Cron.Minutely(), srv => srv.GetTask());
        }
    }
}