using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Emails
{
    public class EmailGroupFromFileCreator : BaseEccWorker
    {
        IEccFilePathMapper FilePathMapper { get; }

        IEccFileEmailsExtractor EmailsExtractor { get; }


        public EmailGroupFromFileCreator(ICrocoAmbientContext context, IEccFileEmailsExtractor emailsExtractor, IEccFilePathMapper filePathMapper) : base(context)
        {
            EmailsExtractor = emailsExtractor;
            FilePathMapper = filePathMapper;
        }

        
        public async Task<BaseApiResponse> CreateEmailGroupFromFile(CreateEmailGroupFromFile model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var filePath = FilePathMapper.MapPath(model.FileName);

            if (!File.Exists(filePath))
            {
                return new BaseApiResponse(false, $"Файл не найден по пути {filePath}");
            }

            var emails = EmailsExtractor.ExtractEmailsListFromFile(filePath);

            var createGroupResult = await new EmailGroupWorker(AmbientContext).CreateGroup(model);

            if (!createGroupResult.IsSucceeded)
            {
                return createGroupResult;
            }

            CreateHandled(emails.Select(x => new EmailInEmailGroupRelation
            {
                EmailGroupId = createGroupResult.ResponseObject
            }));

            return await TrySaveChangesAndReturnResultAsync("Создана группа эмейлов и экспортированы данные из файла");
        }
    }
}