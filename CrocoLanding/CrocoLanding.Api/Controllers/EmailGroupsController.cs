using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Implementation.TaskGivers;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Emails;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    /// <summary>
    /// Группы для эмейлов
    /// </summary>
    [Route("Api/EmailGroup")]
    public class EmailGroupsController : BaseApiController
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public EmailGroupsController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, IEccPixelUrlProvider urlProvider) : base(context, signInManager, userManager, null)
        {
            UrlProvider = urlProvider;
        }

        EmailGroupSender Sender => new EmailGroupSender(SystemAmbientContext, UrlProvider);

        EmailGroupWorker EmailGroupWorker => new EmailGroupWorker(SystemAmbientContext);

        EmailGroupFromFileCreator EmailGroupFromFileCreator => new EmailGroupFromFileCreator(SystemAmbientContext, new AppEccEmailListExtractor());

        public IEccPixelUrlProvider UrlProvider { get; }

        [HttpPost("Test")]
        public BaseApiResponse Test(string jobId, string cronExpression)
        {
            var manager = new RecurringJobManager();

            manager.AddOrUpdate(jobId, Job.FromExpression<SendEmailTaskGiver>(t => t.GetTask()), cronExpression);

            BackgroundJob.Enqueue<SendEmailTaskGiver>(taskGiver => taskGiver.GetTask());

            return new BaseApiResponse(true, "Ok");
        }

        [HttpPost("Send")]
        public Task<BaseApiResponse> Send(SendMailsForEmailGroup model)
        {
            return Sender.StartEmailDistributionForGroup(model);
        }

        /// <summary>
        /// Создать группу эмейлов из файла
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateGroupFromFile")]
        public Task<BaseApiResponse> CreateGroupFromFile([FromBody]CreateEmailGroupFromFile model)
            => EmailGroupFromFileCreator.CreateEmailGroupFromFile(model);


        /// <summary>
        /// Получить группы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public Task<GetListResult<EmailGroupModel>> GetGroups([FromBody]GetListSearchModel model) => EmailGroupWorker.GetEmailGroups(model);


        /// <summary>
        /// Создать группу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public Task<BaseApiResponse<string>> CreateGroup([FromBody]CreateEmailGroup model) => EmailGroupWorker.CreateGroup(model);

        /// <summary>
        /// Добавить адрес в группу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("AddEmail")]
        public Task<BaseApiResponse> AddEmailToGroup([FromBody]AddEmailToEmailGroup model) => EmailGroupWorker.AddEmailToGroup(model);
    }
}