using Croco.Core.Abstractions.Models;
using Croco.Core.Application;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Implementation;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrocoLanding.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class ScheduleController : BaseApiController
    {
        public ScheduleController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : base(context, signInManager, userManager, httpContextAccessor)
        {
        }


        [HttpPost("MonitoringApi")]
        public object Monitoring()
        {
            return JobStorage.Current.GetMonitoringApi().ScheduledJobs(0, 50);
        }

        /// <summary>
        /// Активиравать фоновые задачи
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(ActivateJobs))]
        public BaseApiResponse ActivateJobs(string password)
        {
            if (password != ApiConsts.Password)
            {
                return new BaseApiResponse(false, "Пароль указан неверно");
            }

            var app = CrocoApp.Application;
            var jobManager = app.JobManager;

            foreach (var job in jobManager.GetJobs())
            {
                jobManager.RemoveJob(job.JobId);
            }

            EccServiceRegistrator.AddJobs(app);

            return new BaseApiResponse(true, "Добавлено");
        }
    }
}