using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using CrocoLanding.Api.Controllers.Base;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Implementation.Services;
using Ecc.Logic.Workers.Emails;
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
        public EmailGroupsController(LandingDbContext context, ApplicationSignInManager signInManager, ApplicationUserManager userManager) : base(context, signInManager, userManager, null)
        {
        }

        EmailGroupWorker EmailGroupWorker => new EmailGroupWorker(AmbientContext);

        EmailGroupFromFileCreator EmailGroupFromFileCreator => new EmailGroupFromFileCreator(AmbientContext, new AppEccEmailListExtractor(), new AppEccFilePathMapper());

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