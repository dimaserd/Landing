using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Logic.Workers;
using Ecc.Model.Entities.LinkCatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecc.Implementation.Services
{
    public class RedirectToUrlService : BaseCrocoWorker
    {
        public RedirectToUrlService(ICrocoAmbientContext context) : base(context)
        {
        }

        public async Task<BaseApiResponse<string>> GetUrlToRedirect(string id)
        {
            var redir = await Query<EmailLinkCatch>().FirstOrDefaultAsync(x => x.Id == id);

            if(redir == null)
            {
                return new BaseApiResponse<string>(false, "Редирект на адрес не найден по указзанному идентификатору");
            }

            CreateHandled(new EmailLinkCatchRedirect
            {
                EmailLinkCatchId = id,
                RedirectedOnUtc = DateTime.UtcNow
            });

            return await TrySaveChangesAndReturnResultAsync("Ok", redir.Url);
        }
    }
}