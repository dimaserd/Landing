using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
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

        
        public async Task<BaseApiResponse> CreateEmailGroupFromFile(CreateEmailGroupFromFile model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var emailsResult = EmailsExtractor.ExtractEmailsListFromFile(model.FilePath);

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