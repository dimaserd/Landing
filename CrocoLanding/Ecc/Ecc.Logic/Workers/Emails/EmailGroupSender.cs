using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Core.Workers;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

            var group = await Query<EmailGroup>().Include(x => x.Emails)
                .FirstOrDefaultAsync(x => x.Id == model.EmailGroupId);

            if (group == null)
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            if (group.Emails.Count == 0)
            {
                return new BaseApiResponse(false, "Эмейлы не существуют в данной группе");
            }

            var sender = new EmailDelayedSender(AmbientContext, UrlProvider);

            return await sender.SendEmails(group.Emails.Select(x => new SendMailMessage
            {
                Email = x.Email,
                Body = model.Body,
                Subject = model.Subject,
                AttachmentFileIds = model.AttachmentFileIds
            }));
        }

    }
}