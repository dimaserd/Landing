using Croco.Core.Abstractions.Models.Log;
using Croco.Core.Application;
using Croco.Core.Implementations.TransactionHandlers;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Contract.Models.Emails;
using Ecc.Implementation.Services;
using Ecc.Implementation.Settings;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Implementation.TaskGivers
{
    public class SendEmailTaskGiverByCallBackRequest
    {
        public async Task GetTask()
        {
            //Получаю неотправленные заявки на перезвон
            var callBacks = await CrocoTransactionHandler.System.ExecuteAndCloseTransaction(amb =>
            {
                var callBackRepo = amb.RepositoryFactory.GetRepository<CallBackRequest>();

                return callBackRepo.Query()
                    .Where(x => !x.IsNotified).ToListAsync();
            });

            var setting = CrocoApp.Application.SettingsFactory.GetSetting<SendGridEmailSettings>();

            //Отправляю сообщения
            var sender = new SendGridEmailSender(setting);

            var results = await sender.SendEmails(callBacks, x => new SendEmailModel
            {
                Body = $"Создана заявка на перезвон '{x.EmailOrPhoneNumber}'",
                Email = "dimaserd84@gmail.com",
                Subject = "Перезвони"
            });

            //Обновляю статусы заявок
            await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(async amb =>
            {
                var callBackRepo = amb.RepositoryFactory.GetRepository<CallBackRequest>();

                foreach (var result in results)
                {
                    var callBack = result.Item1;

                    callBack.IsNotified = result.Item2.IsSucceeded;

                    if (callBack.IsNotified)
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