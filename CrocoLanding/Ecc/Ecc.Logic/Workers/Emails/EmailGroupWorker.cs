using ClosedXML.Excel;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public IEccFilePathMapper FilePathMapper { get; }

        public EmailGroupWorker(ICrocoAmbientContext ambientContext, IEccFilePathMapper filePathMapper) : base(ambientContext)
        {
            FilePathMapper = filePathMapper;
        }

        
        public async Task<BaseApiResponse> SendEmailsToGroup(string groupId)
        {
            var groupWithEmails = await Query<EmailGroup>()
                .Include(x => x.Emails)
                .FirstOrDefaultAsync(x => x.Id == groupId);

            if(groupWithEmails == null)
            {
                return new BaseApiResponse(false, "Группа не найдена по указанному идентификатору");
            }

            return null;
        }

        static List<string> GetEmails(string filePath)
        {
            using var workBook = new XLWorkbook(filePath);

            var sheet = workBook.Worksheets.First();

            var maybeEmails = new List<string>();

            foreach (var row in sheet.Rows())
            {
                var cell = row.Cell(2);

                var email = cell.GetString();

                maybeEmails.Add(email);
            }

            return maybeEmails.Select(x =>
            {
                if (x.Contains(","))
                {
                    return x.Split(",", StringSplitOptions.RemoveEmptyEntries);
                }

                return new[] { x };
            }).SelectMany(x => x)
            .Select(x => x.Trim())
            .Where(x => new EmailAddressAttribute().IsValid(x)).ToList();
        }

        public async Task<BaseApiResponse> CreateEmailGroupFromFile(CreateEmailGroupFromFile model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if(!validation.IsSucceeded)
            {
                return validation;
            }

            var filePath = FilePathMapper.MapPath(model.FileName);

            if(!File.Exists(filePath))
            {
                return new BaseApiResponse(false, $"Файл не найден по пути {filePath}");
            }

            var emails = GetEmails(filePath);

            var createGroupResult = await CreateGroup(model);

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