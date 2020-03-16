using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailGroupFromFileCreator : BaseEccWorker
    {
        IEccFileEmailsExtractor EmailsExtractor { get; }

        public EmailGroupFromFileCreator(ICrocoAmbientContext context, IEccFileEmailsExtractor emailsExtractor) : base(context)
        {
            EmailsExtractor = emailsExtractor;
        }

        public async Task<BaseApiResponse> ApppendEmailsToGroup(AppendEmailsFromFileToGroup model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var emailGroup = await Query<EmailGroup>().Include(x => x.Emails).FirstOrDefaultAsync(x => x.Id == model.EmailGroupId);

            if(emailGroup == null)
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            var emailsResult = await EmailsExtractor.ExtractEmailsListFromFile(model.FilePath);

            if (!emailsResult.IsSucceeded)
            {
                return new BaseApiResponse(emailsResult);
            }

            var emailsInGroup = emailGroup.Emails.Select(x => x.Email).ToList();

            var emailsToAddToGroup = emailsResult.ResponseObject.Where(x => !emailsInGroup.Contains(x)).Select(x => new EmailInEmailGroupRelation
            {
                EmailGroupId = model.EmailGroupId,
                Email = x,
                Id = Guid.NewGuid().ToString()
            }).ToList();

            CreateHandled(emailsToAddToGroup);

            return await TrySaveChangesAndReturnResultAsync($"В группу emailов '{emailGroup.Name}' добавлено {emailsToAddToGroup.Count} новых электронных адресов");
        }

        public async Task<BaseApiResponse> CreateEmailGroupFromFile(CreateEmailGroupFromFile model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var emailsResult = await EmailsExtractor.ExtractEmailsListFromFile(model.FilePath);

            if(!emailsResult.IsSucceeded)
            {
                return new BaseApiResponse(emailsResult);
            }

            var createGroupResult = await new EmailGroupWorker(AmbientContext).CreateGroup(model);

            if (!createGroupResult.IsSucceeded)
            {
                return createGroupResult;
            }

            CreateHandled(emailsResult.ResponseObject.Select(x => new EmailInEmailGroupRelation
            {
                EmailGroupId = createGroupResult.ResponseObject
            }));

            return await TrySaveChangesAndReturnResultAsync("Создана группа эмейлов и экспортированы данные из файла");
        }
    }
}