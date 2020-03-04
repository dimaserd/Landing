using System;
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
    public class MailMessageSender : BaseCrocoWorker
    {
        IEccFileService FileService { get; }

        public MailMessageSender(ICrocoAmbientContext ambientContext, IEccFileService fileService) : base(ambientContext)
        {
            FileService = fileService;
        }

        private async Task<IEmailSender> GetEmailSender(int[] fileIds)
        {
            if (!(Application is IEccApplication eccApp))
            {
                var ex = new Exception($"Глобальное приложение не реализует интерфейс {nameof(IEccApplication)}");

                Logger.LogException(ex);

                throw ex;
            }

            return eccApp.GetEmailSender(new GetEmailSenderOptions
            {
                AmbientContext = AmbientContext,
                //Устанавливаю вложения, получая их из базы данных
                Attachments = await FileService.GetFileDatas(fileIds)
            });
        }

        public async Task<List<UpdateInteractionStatus>> SendInteractions(List<SendEmailModelWithInteractionId> messages)
        {
            var fileIds = messages.SelectMany(x => x.EmailModel.AttachmentFileIds).ToArray();

            var sender = await GetEmailSender(fileIds);

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