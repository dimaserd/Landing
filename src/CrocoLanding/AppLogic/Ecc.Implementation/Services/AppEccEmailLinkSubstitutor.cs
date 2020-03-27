using Ecc.Logic.Abstractions;
using Ecc.Model.Entities.LinkCatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ecc.Implementation.Services
{
    public class AppEccEmailLinkSubstitutor : IEccEmailLinkSubstitutor
    {
        /// <summary>
        /// Url вида https://crocosoft.ru/UrlCatcher/Redirect/{0}
        /// </summary>
        string UrlRedirectFormat { get; }
        HashSet<string> UrlDomains { get; }

        public AppEccEmailLinkSubstitutor(string urlRedirectFormat, HashSet<string> urlDomains)
        {
            UrlRedirectFormat = urlRedirectFormat;
            UrlDomains = urlDomains;
        }

        public string GetUrlById(string id)
        {
            return string.Format(UrlRedirectFormat, id);
        }

        static string SubstituteMatch(string text, Match match)
        {
            return text.Substring(0, match.Index) + match.Value + text.Substring(match.Index + match.Length);
        }

        public (string, EmailLinkCatch[]) ProcessEmailText(string body, string mailMessageId)
        {
            var list = new HashSet<(string, string)>();

            var regex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var matches = regex.Matches(body).ToList();

            foreach(var match in matches)
            {
                if (UrlDomains.Any(x => match.Value.Contains(x)))
                {
                    var id = Guid.NewGuid().ToString();

                    var url = GetUrlById(id);

                    list.Add((id, match.Value));

                    body = SubstituteMatch(body, match);
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