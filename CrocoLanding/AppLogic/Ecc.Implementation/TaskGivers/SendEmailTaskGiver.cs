using Croco.Core.Implementations.TransactionHandlers;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Messaging;
using System.Threading.Tasks;

namespace Ecc.Implementation.TaskGivers
{
    public class SendEmailTaskGiver
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
            return CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
            {
                return new MailDistributionInteractionWorker(amb, FileService, SenderProvider).SendEmailsAsync();
            });
        }
    }
}