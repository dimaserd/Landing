using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Logic.Workers;
using Ecc.Contract.Models;
using Ecc.Logic.Abstractions;
using Ecc.Model.Consts;
using Ecc.Model.Entities.External;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Entities.LinkCatch;
using Ecc.Model.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Core.Workers
{
    public class EmailDelayedSender : BaseCrocoWorker
    {
        IEccPixelUrlProvider UrlProvider { get; }
        IEccEmailLinkSubstitutor EmailLinkSubstitutor { get; }

        public EmailDelayedSender(ICrocoAmbientContext ambientContext, IEccPixelUrlProvider urlProvider, IEccEmailLinkSubstitutor emailLinkSubstitutor) : base(ambientContext)
        {
            UrlProvider = urlProvider;
            EmailLinkSubstitutor = emailLinkSubstitutor;
        }


        public async Task<BaseApiResponse> SendEmail(SendMailMessage message)
        {
            var toMes = ToMailMessage(message);

            GetRepository<MailMessageInteraction>().CreateHandled(toMes.Item1);
            GetRepository<InteractionStatusLog>().CreateHandled(toMes.Item2);
            GetRepository<InteractionAttachment>().CreateHandled(toMes.Item3);
            GetRepository<EmailLinkCatch>().CreateHandled(toMes.Item4);

            return await TrySaveChangesAndReturnResultAsync("Email-сообщение добавлено в очередь");
        }

        public async Task<BaseApiResponse> SendEmails(IEnumerable<SendMailMessage> messages)
        {
            var interations = messages.Select(ToMailMessage).ToList();

            GetRepository<MailMessageInteraction>().CreateHandled(interations.Select(x => x.Item1));
            GetRepository<InteractionStatusLog>().CreateHandled(interations.Select(x => x.Item2));
            GetRepository<InteractionAttachment>().CreateHandled(interations.SelectMany(x => x.Item3));
            GetRepository<EmailLinkCatch>().CreateHandled(interations.SelectMany(x => x.Item4));

            return await TrySaveChangesAndReturnResultAsync("Email-сообщения добавлено в очередь");
        }


        public async Task<BaseApiResponse> SendEmail(SendMailMessageToUser message)
        {
            var emailAndId = await Query<EccUser>().Select(x => new { x.Email, x.Id })
                .FirstOrDefaultAsync(x => x.Id == message.UserId);

            if (emailAndId == null)
            {
                return new BaseApiResponse(false, "Клиент не найден по указанному идентификатору");
            }

            var toMes = ToMailMessage(message, emailAndId.Email);

            GetRepository<MailMessageInteraction>().CreateHandled(toMes.Item1);
            GetRepository<InteractionStatusLog>().CreateHandled(toMes.Item2);
            GetRepository<InteractionAttachment>().CreateHandled(toMes.Item3);

            return await TrySaveChangesAndReturnResultAsync("Email-сообщение добавлено в очередь");
        }

        public async Task<BaseApiResponse> SendEmails(IEnumerable<SendMailMessageToUser> messages)
        {
            var userIds = messages.Select(x => x.UserId).ToList();

            var emailAndIds = await Query<EccUser>()
                .Select(x => new { x.Email, x.Id })
                .Where(x => userIds.Contains(x.Id))
                .ToListAsync();

            var mailMes = messages.Select(x =>
            {
                var emailAndId = emailAndIds.FirstOrDefault(z => z.Id == x.UserId);

                if (emailAndId != null)
                {
                    return ToMailMessage(x, emailAndId.Email);
                }

                return (null, null, null, null);
            });

            var toCreate = mailMes.Where(x => x.Item1 != null);

            GetRepository<MailMessageInteraction>().CreateHandled(toCreate.Select(x => x.Item1));
            GetRepository<InteractionStatusLog>().CreateHandled(toCreate.Select(x => x.Item2));
            GetRepository<InteractionAttachment>().CreateHandled(toCreate.SelectMany(x => x.Item3));
            GetRepository<EmailLinkCatch>().CreateHandled(toCreate.SelectMany(x => x.Item4));

            return await TrySaveChangesAndReturnResultAsync("Email-сообщения добавлены в очередь");
        }

        private static List<InteractionAttachment> GetAttachments(string id, List<int> attachmentFileIds)
        {
            var attachments = attachmentFileIds?.Select(x => new InteractionAttachment
            {
                FileId = x,
                InteractionId = id,
            }).ToList();

            return attachments ?? new List<InteractionAttachment>();
        }

        private (MailMessageInteraction, InteractionStatusLog, List<InteractionAttachment>, EmailLinkCatch[]) ToMailMessage(SendMailMessageToUser message, string email)
        {
            var id = Guid.NewGuid().ToString();

            var res = EmailLinkSubstitutor.ProcessEmailText(message.Body, id);

            return (new MailMessageInteraction
            {
                Id = id,
                TitleText = message.Subject,
                MessageText = AddReadingLink(res.Item1, id),
                SendNow = true,
                UserId = message.UserId,
                Type = EccConsts.EmailType,
                ReceiverEmail = email,
            },
            new InteractionStatusLog
            {
                InteractionId = id,
                Status = InteractionStatus.Created,
                StartedOn = Application.DateTimeProvider.Now
            },
            GetAttachments(id, message.AttachmentFileIds),
            res.Item2);
        }

        public (MailMessageInteraction, InteractionStatusLog, List<InteractionAttachment>, EmailLinkCatch[]) ToMailMessage(SendMailMessage message)
        {
            var id = Guid.NewGuid().ToString();

            var res = EmailLinkSubstitutor.ProcessEmailText(message.Body, id);

            return (new MailMessageInteraction
            {
                Id = id,
                TitleText = message.Subject,
                MessageText = AddReadingLink(res.Item1, id),
                ReceiverEmail = message.Email,
                SendNow = true,
                UserId = null,
                Type = EccConsts.EmailType
            },
            new InteractionStatusLog
            {
                InteractionId = id,
                Status = InteractionStatus.Created,
                StartedOn = Application.DateTimeProvider.Now
            },
            GetAttachments(id, message.AttachmentFileIds), res.Item2);
        }

        public string AddReadingLink(string body, string id)
        {
            return body += $"<img src=\"{UrlProvider.GetPixelEmailUrl(id)}\" height=\"1\" width=\"1\" />";
        }
    }
}