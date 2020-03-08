﻿using Croco.Core.Abstractions.Application;
using Croco.Core.Extensions;
using Ecc.Implementation.TaskGivers;
using Hangfire;

namespace Ecc.Logic
{
    public static class EccServiceRegistrator
    {
        public static void AddJobs(ICrocoApplication application)
        {
            application.AddJob<SendEmailTaskGiverByCallBackRequest>(Cron.Minutely());
        }
    }
}