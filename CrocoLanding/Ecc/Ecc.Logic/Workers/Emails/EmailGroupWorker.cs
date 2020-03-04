using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
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

        public async Task<BaseApiResponse> AddEmailToGroup(AddEmailToEmailGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if(!await Query<EmailGroup>().AnyAsync(x => x.Id == model.EmailGroupId))
            {
                return new BaseApiResponse(false, "Группа для эмелов не найдена по указанному идентификатору");
            }

            var repo = GetRepository<EmailInEmailGroupRelation>();

            if (await repo.Query().AnyAsync(x => x.EmailGroupId == model.EmailGroupId && x.Email == model.Email))
            {
                return new BaseApiResponse(false, "Эмейл уже добавлен к данной группе");
            }

            repo.CreateHandled(new EmailInEmailGroupRelation
            {
                Email = model.Email,
                EmailGroupId = model.EmailGroupId
            });

            return await TrySaveChangesAndReturnResultAsync("Email добавлен в группу");
        }
    }
}