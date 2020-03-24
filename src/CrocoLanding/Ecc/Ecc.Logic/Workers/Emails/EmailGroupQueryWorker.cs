using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models.Search;
using Croco.Core.Logic.Workers;
using Croco.Core.Search.Extensions;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Model.Entities.Email;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailGroupQueryWorker : LightCrocoWorker
    {
        public EmailGroupQueryWorker(ICrocoAmbientContext context) : base(context)
        {
        }

        static readonly Expression<Func<EmailGroup, EmailGroupModel>> SelectExpression = x => new EmailGroupModel
        {
            Id = x.Id,
            Name = x.Name,
            EmailsCount = x.Emails.Count
        };

        public Task<GetListResult<string>> GetEmailsInGroup(GetEmailsInGroup model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailInEmailGroupRelation>().Where(x => x.EmailGroupId == model.EmailGroupId).OrderByDescending(x => x.Email), x => x.Email);
        }

        public Task<GetListResult<EmailGroupModel>> GetEmailGroups(GetListSearchModel model)
        {
            return EFCoreExtensions.GetAsync(model, Query<EmailGroup>().OrderByDescending(x => x.CreatedOn), SelectExpression);
        }
    }
}