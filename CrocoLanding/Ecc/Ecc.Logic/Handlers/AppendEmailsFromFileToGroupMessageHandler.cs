using Croco.Core.EventSourcing.Implementations;
using Croco.Core.Implementations.TransactionHandlers;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    public class AppendEmailsFromFileToGroupMessageHandler : CrocoMessageHandler<AppendEmailsFromFileToGroup>
    {
        const int CountInPack = 100;

        IEccFileEmailsExtractor EmailsExtractor { get; }

        public AppendEmailsFromFileToGroupMessageHandler(IEccFileEmailsExtractor emailsExtractor)
        {
            EmailsExtractor = emailsExtractor;
        }
        
        public override async Task HandleMessage(AppendEmailsFromFileToGroup model)
        {
            var emailsSafeValue = await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
            {
                return amb.RepositoryFactory.Query<EmailInEmailGroupRelation>()
                    .Where(x => x.EmailGroupId == model.EmailGroupId)
                    .Select(x => x.Email)
                    .ToListAsync();
            });

            if (!emailsSafeValue.IsSucceeded)
            {
                throw new Exception("Ошибка");
            }

            var emailsDict = emailsSafeValue.Value.ToDictionary(x => x);

            var emailsFromFileResult = await EmailsExtractor.ExtractEmailsListFromFile(model.FilePath);

            if (!emailsFromFileResult.IsSucceeded)
            {
                throw new Exception(emailsFromFileResult.Message);
            }

            var newEmails = emailsFromFileResult.ResponseObject.Where(x => emailsDict.ContainsKey(x)).ToList();

            var count = 0;

            while (count < newEmails.Count)
            {
                await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
                {
                    var repo = amb.RepositoryFactory.GetRepository<EmailInEmailGroupRelation>();

                    var emailsToAdd = newEmails.Skip(count).Take(CountInPack).Select(x => new EmailInEmailGroupRelation 
                    {
                        EmailGroupId = model.EmailGroupId,
                        Email = x
                    }).ToList();

                    repo.CreateHandled(emailsToAdd);

                    return amb.RepositoryFactory.SaveChangesAsync();
                });

                count += CountInPack;
            }
        }
    }
}