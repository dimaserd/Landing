using Croco.Core.Abstractions;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Models;
using Ecc.Logic.Workers.Emails;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Enumerations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Messaging
{
    public class MailDistributionInteractionWorker : ApplicationInteractionWorker
    {
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        IEccFileService FileService { get; }
        IEmailSenderProvider SenderProvider { get; }

        public MailDistributionInteractionWorker(ICrocoAmbientContext ambientContext, IEccFileService fileService, IEmailSenderProvider senderProvider) : base(ambientContext)
        {
            FileService = fileService;
            SenderProvider = senderProvider;
        }

        public async Task SendEmailsAsync()
        {
            await semaphore.WaitAsync();

            var interactions = await GetQueryWithStatus(Query<MailMessageInteraction>())
                .Where(x => x.Status == InteractionStatus.Created)
                .Select(x => new SendEmailModelWithInteractionId
                {
                    InteractionId = x.Interaction.Id,
                    EmailModel = new SendEmailModel
                    {
                        Subject = x.Interaction.TitleText,
                        Body = x.Interaction.MessageText,
                        Email = x.Interaction.ReceiverEmail,
                        AttachmentFileIds = x.Interaction.Attachments.Select(t => t.FileId).ToList(),
                    }
                })
                .ToListAsync();

            await SetStatusForInteractions(interactions.Select(x => x.InteractionId), InteractionStatus.InProccess, "In process of sending emails");

            var emailSender = new MailMessageSender(AmbientContext, FileService, SenderProvider);

            var updates = await emailSender.SendInteractions(interactions);

            await UpdateInteractionStatusesAsync(updates);

            semaphore.Release();
        }
    }
}