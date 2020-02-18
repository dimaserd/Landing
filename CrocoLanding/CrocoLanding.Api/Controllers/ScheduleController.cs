using Croco.Core.Abstractions.Models;
using Croco.Core.Application;
using CrocoLanding.Api.Controllers.Base;
using CrocoShop.Model.Contexts;
using Ecc.Logic;
using Microsoft.AspNetCore.Mvc;

namespace CrocoLanding.Api.Controllers
{
    public class ScheduleController : BaseApiController
    {
        public ScheduleController(LandingDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Активиравать фоновые задачи
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(ActivateJobs))]
        public BaseApiResponse ActivateJobs(string password)
        {
            if(password != ApiConsts.Password)
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