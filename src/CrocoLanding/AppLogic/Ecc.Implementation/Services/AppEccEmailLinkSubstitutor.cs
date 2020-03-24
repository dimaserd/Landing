using Ecc.Logic.Abstractions;
using Ecc.Model.Entities.LinkCatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecc.Implementation.Services
{
    public class AppEccEmailLinkSubstitutor : IEccEmailLinkSubstitutor
    {
        /// <summary>
        /// Url вида https://crocosoft.ru/UrlCatcher/Redirect/{0}
        /// </summary>
        string UrlRedirectFormat { get; }
        List<string> UrlsToReplace { get; }

        public AppEccEmailLinkSubstitutor(string urlRedirectFormat, List<string> urlsToReplace)
        {
            UrlRedirectFormat = urlRedirectFormat;
            UrlsToReplace = urlsToReplace;
        }

        public string GetUrlById(string id)
        {
            return string.Format(UrlRedirectFormat, id);
        }

        public (string, EmailLinkCatch[]) ProcessEmailText(string body, string mailMessageId)
        {
            var list = new HashSet<(string, string)>();

            foreach(var toReplace in UrlsToReplace)
            {
                var id = Guid.NewGuid().ToString();

                var url = GetUrlById(id);

                while (body.Contains(toReplace))
                {
                    list.Add((id, toReplace));

                    body = body.Replace(toReplace, url);
                }
            }

            return (body, list.Select(x => new EmailLinkCatch
            {
                Id = x.Item1,
                Url = x.Item2,
                CreatedOnUtc = DateTime.UtcNow,
                MailMessageId = mailMessageId
            }).ToArray());
        }
    }
}