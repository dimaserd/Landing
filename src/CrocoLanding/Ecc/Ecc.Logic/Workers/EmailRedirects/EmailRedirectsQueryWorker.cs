using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models.Search;
using Croco.Core.Logic.Workers;
using Croco.Core.Search.Extensions;
using Ecc.Contract.Models.EmailRedirects;
using Ecc.Model.Entities.LinkCatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.EmailRedirects
{
    public class EmailRedirectsQueryWorker : LightCrocoWorker
    {
        public EmailRedirectsQueryWorker(ICrocoAmbientContext context) : base(context)
        {
        }


        static readonly Expression<Func<EmailLinkCatch, EmailLinkCatchRedirectsCountModel>> SelectExpression = x => new EmailLinkCatchRedirectsCountModel
        {
            Id = x.Id,
            Url = x.Url,
            Count = x.Redirects.Count
        };

        public Task<List<EmailLinkCatchRedirectsCountModel>> GetCatchesByEmailId(string id)
        {
            return Query<EmailLinkCatch>().Where(x => x.MailMessageId == id)
                .OrderByDescending(x => x.CreatedOnUtc).Select(SelectExpression)
                .ToListAsync();
        }

        public Task<GetListResult<EmailLinkCatchRedirectsCountModel>> Query(GetListSearchModel model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailLinkCatch>().OrderByDescending(x => x.CreatedOnUtc), SelectExpression);
        }

        public Task<EmailLinkCatchDetailedModel> GetById(string id)
        {
            return Query<EmailLinkCatch>().Select(x => new EmailLinkCatchDetailedModel
            {
                Id = x.Id,
                RedirectsOn = x.Redirects.Select(t => t.RedirectedOnUtc).ToList(),
                Url = x.Url
            }).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}