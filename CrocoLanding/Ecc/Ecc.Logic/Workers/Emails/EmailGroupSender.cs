using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Emails
{

    public class EmailGroupSender : BaseEccWorker
    {
        IEccPixelUrlProvider UrlProvider { get; }

        public EmailGroupSender(ICrocoAmbientContext context, IEccPixelUrlProvider urlProvider) : base(context)
        {
            UrlProvider = urlProvider;
        }

        public async Task<BaseApiResponse> StartEmailDistributionForGroup(SendMailsForEmailGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (await Query<EmailGroup>().AnyAsync(x => x.Id == model.EmailGroupId))
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            if (await Query<EmailInEmailGroupRelation>().AnyAsync(x => x.Id == model.EmailGroupId))
            {
                return new BaseApiResponse(false, "Эмейлы не существуют в данной группе");
            }

            await PublishMessageAsync(model);

            return new BaseApiResponse(true, "Начата рассылка сообщений по данной группе");
        }
    }
}