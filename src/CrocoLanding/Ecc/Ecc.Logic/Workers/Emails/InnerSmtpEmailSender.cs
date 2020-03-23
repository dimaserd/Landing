using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Files;
using Croco.Core.Abstractions.Models;
using Croco.Core.Abstractions.Models.Log;
using Croco.Core.Logic.Workers;
using Croco.Core.Utils;
using Ecc.Contract.Models;
using Ecc.Contract.Models.Emails;
using Ecc.Contract.Settings;
using Ecc.Logic.Abstractions;

namespace Ecc.Logic.Workers.Emails
{
    public class InnerSmtpEmailSender : BaseCrocoWorker, IEmailSender
    {
        SmtpEmailSettingsModel EmailSettings { get; }

        List<EccFileData> Attachments { get; }

        public InnerSmtpEmailSender(ICrocoAmbientContext ambientContext, SmtpEmailSettingsModel settings, List<EccFileData> attachments = null) : base(ambientContext)
        {
            Attachments = attachments ?? new List<EccFileData>();
            EmailSettings = settings;
        }


        public Task<List<(TModel, BaseApiResponse)>> SendEmails<TModel>(IEnumerable<TModel> messages, Func<TModel, SendEmailModel> mapper)
        {
            return Task.FromResult(SendEmailsInner(messages, mapper));
        }

        private List<(TModel, BaseApiResponse)> SendEmailsInner<TModel>(IEnumerable<TModel> messages, Func<TModel, SendEmailModel> mapper)
        {
            try
            {
                using var smtpMail = GetSmtpClient();

                var result = messages.Select(x => (x, SendSingleEmail(smtpMail, mapper(x)))).ToList();

                return result;
            }
            catch (Exception ex)
            {                
                Logger.LogException("InnerSmtpEmailSender.SendEmailsInner.OnException", ex);
                Logger.LogWarn("InnerSmtpEmailSender.SendEmails.BeforeError", "Произошла ошибка при инициализации SmtpClient при отправке email",
                    new LogNode("EmailSettings", EmailSettings));

                var errorResp = new BaseApiResponse(ex);

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
                    Logger.LogException("InnerSmtpEmailSender.SendSingleEmail.Exception", ex);
                    Logger.LogWarn("InnerSmtpEmailSender.SendSingleEmail.Exception", "Произошла ошибка при отправке emzil сообщения через SmtpClient",
                    new LogNode("EmailSettings", EmailSettings), new LogNode("EmailModel", emailModel));
                    return new BaseApiResponse(ex);
                }
            }

            return new BaseApiResponse(true, "Ok");
        }

        private IFileData[] GetFileDatas(List<int> fileIds)
        {
            if (fileIds == null)
            {
                return null;
            }

            var res = Attachments.Where(x => fileIds.Contains(x.Id)).ToArray();

            if (res.Length < fileIds.Count)
            {
                //Находим идентификаторы файлов, которые не найдены
                var notFoundFileIds = fileIds.Where(x => !res.Select(t => t.Id).Contains(x)).ToList();

                throw new ApplicationException($"Не все вложения были найдены по указанным идентификаторам. Идентификаторы не найденных вложений: {Tool.JsonConverter.Serialize(notFoundFileIds)}");
            }

            return res;
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