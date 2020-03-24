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
        public AppEccEmailLinkSubstitutor(string urlRedirectFormat)
        {
            UrlRedirectFormat = urlRedirectFormat;
        }

        /// <summary>
        /// Url вида https://crocosoft.ru/UrlCatcher/Redirect/{0}
        /// </summary>
        public string UrlRedirectFormat { get; }

        public string GetUrlById(string id)
        {
            return string.Format(UrlRedirectFormat, id);
        }

        public (string, EmailLinkCatch[]) ProcessEmailText(string body)
        {
            var list = new List<(string, string)>();

            var regex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var matches = regex.Matches(body);

            foreach (Match item in matches)
            {
                var id = Guid.NewGuid().ToString();
                list.Add((id, item.Value));
                body = body.Replace(item.Value, GetUrlById(id));
            }

            return (body, list.Select(x => new EmailLinkCatch
            {
                Id = x.Item1,
                Url = x.Item2,
                CreatedOnUtc = DateTime.UtcNow
            }).ToArray());
        }
    }
}