using Croco.Core.Implementations.TransactionHandlers;
using Ecc.Logic.Abstractions;
using Ecc.Model.Entities.LinkCatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public async Task<string> ProcessEmailText(string body)
        {
            var list = new List<(string, string)>();

            var matches = Regex.Matches(body, @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");

            foreach (Match item in matches)
            {
                var id = Guid.NewGuid().ToString();
                list.Add((id, item.Value));
                body = body.Replace(item.Value, GetUrlById(id));
            }

            await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
            {
                var repo = amb.RepositoryFactory.GetRepository<EmailLinkCatch>();

                repo.CreateHandled(list.Select(x => new EmailLinkCatch
                {
                    Id = x.Item1,
                    Url = x.Item2
                }));

                return amb.RepositoryFactory.SaveChangesAsync();
            });

            return body;
        }
    }
}
