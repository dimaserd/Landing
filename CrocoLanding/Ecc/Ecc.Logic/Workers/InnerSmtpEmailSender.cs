using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Files;
using Croco.Core.Abstractions.Loggers.Models;
using Croco.Core.Models;
using CrocoLanding.Controllers;
using CrocoLanding.Logic;
using Ecc.Contract.Models.Emails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Ecc.Logic.Workers
{
    public class InnerSmtpEmailSender : BaseAppWorker
    {
        EmailSettingsModel EmailSettings { get; }

        

        public InnerSmtpEmailSender(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
            EmailSettings = Application.SettingsFactory.GetSetting<EmailSettingsModel>();
        }

        public (TModel, BaseApiResponse) SendEmail<TModel>(TModel message, Func<TModel, SendEmailModel> mapper)
        {
            return SendEmails(new[] { message }, mapper).First();
        }

        public List<(TModel, BaseApiResponse)> SendEmails<TModel>(IEnumerable<TModel> messages, Func<TModel, SendEmailModel> mapper)
        {
            try
            {
                using var smtpMail = GetSmtpClient();

                var result = messages.Select(x => (x, SendSingleEmail(smtpMail, mapper(x)))).ToList();

                return result;
            }
            catch (Exception ex)
            {
                var errorResp = new BaseApiResponse(ex);

                Logger.LogException(ex);
                Logger.LogWarn("InnerSmtpEmailSender.SendEmails.BeforeError", "Произошла ошибка при инициализации SmtpClient при отправке email",
                    new LogNode("EmailSettings", EmailSettings));

                return messages.Select(x => (x, errorResp)).ToList();
            }
        }

        private SmtpClient GetSmtpClient() => new SmtpClient(EmailSettings.SmtpClientString, EmailSettings.SmtpPort)
        {
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(EmailSettings.UserName, EmailSettings.Password),
        };


        private BaseApiResponse SendSingleEmail(SmtpClient smtpClient, SendEmailModel emailModel)
        {
            using (var mail = new MailMessage(new MailAddress(EmailSettings.FromAddress), new MailAddress(emailModel.Email))
            {
                Subject = emailModel.Subject,
                Body = emailModel.Body,
                IsBodyHtml = EmailSettings.IsBodyHtml
            })
            {
                try
                {
                    //Добавляем вложения в письмо
                    AddAttachments(mail, GetFileDatas(emailModel.AttachmentFileIds));
                    //отправляем письмо
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    return new BaseApiResponse(ex);
                }
            }

            return new BaseApiResponse(true, "Ok");
        }

        private IFileData[] GetFileDatas(List<int> fileIds)
        {
            return null;
        }

        private void AddAttachments(MailMessage mail, IFileData[] attachments)
        {
            if (attachments == null)
            {
                return;
            }

            foreach (var attachment in attachments)
            {
                mail.Attachments.Add(GetEmailAttachment(attachment));
            }
        }

        private Attachment GetEmailAttachment(IFileData fileData)
        {
            var ms = new MemoryStream(fileData.Data) { Position = 0 };

            return new Attachment(ms, Path.GetFileName(fileData.FileName));
        }
    }
}