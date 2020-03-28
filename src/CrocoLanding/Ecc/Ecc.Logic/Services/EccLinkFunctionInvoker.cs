using Croco.Core.Abstractions;
using Croco.Core.Extensions;
using Ecc.Model.Entities.LinkCatch;
using System;
using System.Linq;

namespace Ecc.Logic.Services
{
    public class EccLinkFunctionInvoker : IEccTextFunctionInvoker
    {
        string UrlRedirectFormat { get; }

        public EccLinkFunctionInvoker(string urlRedirectFormat)
        {
            UrlRedirectFormat = urlRedirectFormat;
        }

        public string GetUrlById(string id)
        {
            return string.Format(UrlRedirectFormat, id);
        }

        public string ProccessText(string interactionId, EccReplacing replacing, ICrocoAmbientContext ambientContext)
        {
            var firstArg = replacing.Func.Args.FirstOrDefault();

            if (firstArg == null)
            {
                return replacing.TextToReplace;
            }

            var id = Guid.NewGuid().ToString();

            var linkCatch = new EmailLinkCatch
            {
                Id = id,
                Url = GetUrlById(id),
                CreatedOnUtc = DateTime.UtcNow,
                MailMessageId = interactionId
            };

            ambientContext.RepositoryFactory.CreateHandled(linkCatch);

            return linkCatch.Url;
        }
    }
}