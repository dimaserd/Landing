using Croco.Core.Abstractions.RecurringJobs;
using Croco.Core.Implementations.TransactionHandlers;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Messaging;
using System.Threading.Tasks;

namespace Ecc.Implementation.TaskGivers
{
    public class SendEmailTaskGiver : ICrocoTaskGiver
    {
        public SendEmailTaskGiver(IEccFileService fileService, IEmailSenderProvider senderProvider)
        {
            FileService = fileService;
            SenderProvider = senderProvider;
        }

        IEccFileService FileService { get; }
        IEmailSenderProvider SenderProvider { get; }

        public Task GetTask()
        {
            if(2 > 0)
            {

            }

            return CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
            {
                return new MailDistributionInteractionWorker(amb, FileService, SenderProvider).SendEmailsAsync();
            });
        }
    }
}