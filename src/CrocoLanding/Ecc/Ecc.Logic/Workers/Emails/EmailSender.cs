using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions;
using Croco.Core.Logic.Workers;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Models;
using Ecc.Logic.Models.Interactions;
using Ecc.Model.Enumerations;

namespace Ecc.Logic.Workers.Emails
{
    /// <summary>
    /// Класс посылающий письма
    /// </summary>
    public class SmtpEmailSender : BaseCrocoWorker
    {
        IEccFileService FileService { get; }
        IEmailSenderProvider SenderProvider { get; }

        public SmtpEmailSender(ICrocoAmbientContext ambientContext, IEccFileService fileService, IEmailSenderProvider senderProvider) : base(ambientContext)
        {
            FileService = fileService;
            SenderProvider = senderProvider;
        }

        public async Task<List<UpdateInteractionStatus>> SendInteractions(List<SendEmailModelWithInteractionId> messages)
        {
            var fileIds = messages.SelectMany(x => x.EmailModel.AttachmentFileIds).ToArray();

            var sender = SenderProvider.GetEmailSender(new GetEmailSenderOptions 
            {
                AmbientContext = AmbientContext,
                //Устанавливаю вложения, получая их из базы данных
                Attachments = await FileService.GetFileDatas(fileIds)
            });

            var res = await sender.SendEmails(messages, x => x.EmailModel);

            return res.Select(x => new UpdateInteractionStatus
            {
                Id = x.Item1.InteractionId,
                Status = x.Item2.IsSucceeded ? InteractionStatus.Sent : InteractionStatus.Error,
                StatusDescription = x.Item2.IsSucceeded ? null : x.Item2.Message
            }).ToList();
        }
    }
}