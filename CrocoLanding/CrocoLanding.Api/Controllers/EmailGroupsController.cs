using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Implementation.Services;
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
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        public EmailGroupsController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager, 
            IEccPixelUrlProvider urlProvider, IEccFilePathMapper filePathMapper) : base(context, signInManager, userManager, null)
        {
            UrlProvider = urlProvider;
            FilePathMapper = filePathMapper;
        }

        EmailGroupSender Sender => new EmailGroupSender(SystemAmbientContext, UrlProvider);

        EmailGroupWorker EmailGroupWorker => new EmailGroupWorker(SystemAmbientContext);

        EmailGroupFromFileCreator EmailGroupFromFileCreator => new EmailGroupFromFileCreator(SystemAmbientContext, new AppEccEmailListExtractor());

        public IEccPixelUrlProvider UrlProvider { get; }
        public IEccFilePathMapper FilePathMapper { get; }

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
        [HttpPost("AddEmails")]
        public Task<BaseApiResponse> AddEmailToGroup([FromBody]AddEmailsToEmailGroup model) => EmailGroupWorker.AddEmailsToGroup(model);
    }
}