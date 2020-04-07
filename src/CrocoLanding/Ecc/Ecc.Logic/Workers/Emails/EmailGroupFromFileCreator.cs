using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
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

            if (!await Query<EmailGroup>().AnyAsync(x => x.Id == model.EmailGroupId))
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            if (!EmailsExtractor.FileExists(model.FilePath))
            {
                return new BaseApiResponse(false, "Файл не существует");
            }

            //Отложено добавляем
            await PublishMessageAsync(model);
            
            return await TrySaveChangesAndReturnResultAsync($"В группу emailов начали добавляться новые электронные адреса");
        }
    }
}