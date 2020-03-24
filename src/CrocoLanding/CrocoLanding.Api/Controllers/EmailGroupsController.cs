using Croco.Core.Abstractions.Models;
using Croco.Core.Abstractions.Models.Search;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Extensions;
using Ecc.Logic.Workers.Emails;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CrocoLanding.Api.Controllers
{
    /// <summary>
    /// Группы для эмейлов
    /// </summary>
    [Route("api/[controller]"), ApiController]
    public class EmailGroupsController : BaseApiController
    {
        IEccFilePathMapper FilePathMapper { get; }
        IEccFileEmailsExtractor FileEmailsExtractor { get; }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public EmailGroupsController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, 
            IEccFilePathMapper filePathMapper, IEccFileEmailsExtractor fileEmailsExtractor) : base(context, signInManager, userManager, null)
        {
            FilePathMapper = filePathMapper;
            FileEmailsExtractor = fileEmailsExtractor;
        }

        EmailGroupSender Sender => new EmailGroupSender(AmbientContext);

        EmailGroupWorker EmailGroupWorker => new EmailGroupWorker(AmbientContext);

        EmailGroupQueryWorker QueryWorker => new EmailGroupQueryWorker(AmbientContext);

        EmailGroupFromFileCreator EmailGroupFromFileCreator => new EmailGroupFromFileCreator(AmbientContext, FileEmailsExtractor);

        [HttpPost("Send")]
        public Task<BaseApiResponse> Send(SendMailsForEmailGroup model)
        {
            return Sender.StartEmailDistributionForGroup(model);
        }

        [HttpPost("Remove")]
        public Task<BaseApiResponse> RemoveGroup(string id)
        {
            return EmailGroupWorker.RemoveGroup(id);
        }

        [HttpPost("Send/ViaTemplate")]
        public async Task<BaseApiResponse> SendViaTemplate(string emailGroupId, int? count = 50, int offSet = 0)
        {
            var testModel = EccController.GetTestModel("somemail@mail.com");

            var nModel = testModel.ToSendEmailModel(FilePathMapper);

            if(!nModel.IsSucceeded)
            {
                return nModel;
            }

            var m = nModel.ResponseObject;

            return await Sender.StartEmailDistributionForGroup(new SendMailsForEmailGroup 
            {
                Body = m.Body,
                Subject = m.Subject,
                Count = count,
                EmailGroupId = emailGroupId,
                OffSet = offSet
            });
        }

        [HttpPost("AppendEmails")]
        public Task<BaseApiResponse> AppendEmails([FromBody]AppendEmailsFromFileToGroup model) => EmailGroupFromFileCreator.ApppendEmailsToGroup(model);


        /// <summary>
        /// Получить группы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public Task<GetListResult<EmailGroupModel>> GetGroups([FromBody]GetListSearchModel model) => QueryWorker.GetEmailGroups(model);

        /// <summary>
        /// Получить группы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetEmailsInGroup")]
        public Task<GetListResult<string>> GetEmailsInGroup([FromBody]GetEmailsInGroup model) => QueryWorker.GetEmailsInGroup(model);



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
        [HttpPost("AddEmails")]
        public Task<BaseApiResponse> AddEmailToGroup([FromBody]AddEmailsToEmailGroup model) => EmailGroupWorker.AddEmailsToGroup(model);
    }
}