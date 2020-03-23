using Croco.Core.Abstractions.Models;
using Croco.Core.EventSourcing.Implementations;
using Croco.Core.Implementations.TransactionHandlers;
using Ecc.Contract.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Core.Workers;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    public class SendMailsForEmailGroupMessageHandler : CrocoMessageHandler<SendMailsForEmailGroup>
    {
        IEccPixelUrlProvider UrlProvider { get; }
        IEccEmailLinkSubstitutor EmailLinkSubstitutor { get; }

        const int CountInPack = 100;

        public SendMailsForEmailGroupMessageHandler(IEccPixelUrlProvider urlProvider, IEccEmailLinkSubstitutor emailLinkSubstitutor)
        {
            UrlProvider = urlProvider;
            EmailLinkSubstitutor = emailLinkSubstitutor;
        }


        public async Task<BaseApiResponse> StartEmailDistribution(SendMailsForEmailGroup model)
        {
            var eamilsInGroupSafeValue = await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
            {
                IQueryable<EmailInEmailGroupRelation> initQuery = amb.RepositoryFactory.Query<EmailInEmailGroupRelation>()
                    .Where(x => x.EmailGroupId == model.EmailGroupId)
                    .OrderBy(x => x.Email);
                
                if(model.Count.HasValue && model.Count.Value > 0)
                {
                    initQuery = initQuery.Skip(model.OffSet)
                        .Take(model.Count.Value);
                }

                return initQuery.Select(x => x.Email).ToListAsync();
            });
                
            if (!eamilsInGroupSafeValue.IsSucceeded)
            {
                throw new Exception("Не удалось получить список эмейлов из группы");
            }

            var emails = eamilsInGroupSafeValue.Value;

            var count = 0;

            while (count < emails.Count)
            {
                await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
                {
                    var sender = new EmailDelayedSender(amb, UrlProvider, EmailLinkSubstitutor);

                    return sender.SendEmails(emails.Skip(count).Take(CountInPack).Select(x => new SendMailMessage
                    {
                        Email = x,
                        Body = model.Body,
                        Subject = model.Subject,
                        AttachmentFileIds = model.AttachmentFileIds
                    }));
                });

                count += CountInPack;
            }

            return new BaseApiResponse(true, "Ok");
        }

        public override Task HandleMessage(SendMailsForEmailGroup model)
        {
            return StartEmailDistribution(model);
        }
    }
}