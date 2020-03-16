using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailGroupWorker : BaseEccWorker
    {
        static readonly Expression<Func<EmailGroup, EmailGroupModel>> SelectExpression = x => new EmailGroupModel
        {
            Id = x.Id,
            Name = x.Name
        };

        public EmailGroupWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public Task<GetListResult<EmailGroupModel>> GetEmailGroups(GetListSearchModel model)
        {
            return GetListResult<EmailGroupModel>.GetAsync(model, Query<EmailGroup>().OrderByDescending(x => x.CreatedOn), SelectExpression);
        }

        public async Task<BaseApiResponse<string>> CreateGroup(CreateEmailGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if(!validation.IsSucceeded)
            {
                return new BaseApiResponse<string>(validation);
            }

            var repo = GetRepository<EmailGroup>();

            if(await repo.Query().AnyAsync(x => x.Name == model.Name))
            {
                return new BaseApiResponse<string>(false, $"Группа эмейлов с именем '{model.Name}' уже создана");
            }

            var id = Guid.NewGuid().ToString();

            repo.CreateHandled(new EmailGroup
            {
                Id = id,
                Name = model.Name
            });

            return await TrySaveChangesAndReturnResultAsync("Группа эмейлов создана", id);
        }

        public async Task<BaseApiResponse> AddEmailsToGroup(AddEmailsToEmailGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var emailGroup = await Query<EmailGroup>().Include(x => x.Emails).FirstOrDefaultAsync(x => x.Id == model.EmailGroupId);

            if (emailGroup == null)
            {
                return new BaseApiResponse(false, "Группа для эмелов не найдена по указанному идентификатору");
            }

            var emailsInGroup = emailGroup.Emails.Select(x => x.Email).ToDictionary(x => x);

            var emailsToAdd = model.Emails.Where(x => !emailsInGroup.ContainsKey(x)).Select(x => new EmailInEmailGroupRelation
            {
                Id = Guid.NewGuid().ToString(),
                EmailGroupId = emailGroup.Id,
                Email = x
            }).ToList();

            CreateHandled(emailsToAdd);

            return await TrySaveChangesAndReturnResultAsync("Электронные адреса добавлены в группу");
        }
    }
}