using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Logic.Workers;
using Ecc.Contract.Models;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Core.Workers;
using Ecc.Logic.Extensions;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Messaging
{
    public class EmailSender : BaseCrocoWorker
    {
        IEccPixelUrlProvider UrlProvider { get; }
        IEccFilePathMapper FilePathMapper { get; }
        IEccEmailLinkSubstitutor EmailLinkSubstitutor { get; }

        public EmailSender(ICrocoAmbientContext ambientContext, IEccPixelUrlProvider urlProvider, IEccFilePathMapper filePathMapper, IEccEmailLinkSubstitutor emailLinkSubstitutor) : base(ambientContext)
        {
            UrlProvider = urlProvider;
            FilePathMapper = filePathMapper;
            EmailLinkSubstitutor = emailLinkSubstitutor;
        }

        
        public async Task<BaseApiResponse> SendEmailViaTemplate(SendMailMessageViaHtmlTemplate model)
        {
            var sendModel = model.ToSendEmailModel(FilePathMapper);

            if (!sendModel.IsSucceeded)
            {
                return new BaseApiResponse(sendModel);
            }

            var sender = new EmailDelayedSender(AmbientContext, UrlProvider, EmailLinkSubstitutor);

            var resp = sendModel.ResponseObject;

            return await sender.SendEmail(new SendMailMessage
            {
                Body = resp.Body,
                Email = resp.Email,
                AttachmentFileIds = resp.AttachmentFileIds,
                Subject = resp.Subject,
            });
        }
    }
}