using Croco.Core.Abstractions.Loggers.Models;
using Croco.Core.Abstractions.RecurringJobs;
using Croco.Core.Implementations.TransactionHandlers;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Contract.Models.Emails;
using Ecc.Logic.Workers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.TaskGivers
{
    public class SendEmailTaskGiver : ICrocoTaskGiver
    {
        public Task GetTask()
        {
            return CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(async amb =>
            {
                var callBackRepo = amb.RepositoryFactory.GetRepository<CallBackRequest>();

                var callBacks = await callBackRepo.Query()
                    .Where(x => !x.IsNotified).ToListAsync();

                var sender = new InnerSmtpEmailSender(amb);

                foreach(var callBack in callBacks)
                {
                    var result = sender.SendEmail(new SendEmailModel
                    {
                        Body = $"Создана заявка на перезвон '{callBack.EmailOrPhoneNumber}'",
                        Email = "dimaserd84@gmail.com",
                        Subject = "Перезвони"
                    }, x => x);

                    callBack.IsNotified = result.Item2.IsSucceeded;

                    if(callBack.IsNotified)
                    {
                        callBackRepo.UpdateHandled(callBack);
                    }
                    else
                    {
                        amb.Logger.LogWarn("SendEmailTaskGiver.GetTask.SendEmail.IsNotSucceeded", "Письмо не отправилось", new LogNode("Message", result.Item1));
                    }
                }

                await amb.RepositoryFactory.SaveChangesAsync();
            });
        }
    }
}