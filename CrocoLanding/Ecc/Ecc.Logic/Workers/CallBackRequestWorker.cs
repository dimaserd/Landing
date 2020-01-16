﻿using Croco.Core.Abstractions;
using Croco.Core.Implementations.AmbientContext;
using Croco.Core.Implementations.TransactionHandlers;
using Croco.Core.Models;
using CrocoLanding.Logic;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Contract.Models;
using Ecc.Contract.Models.Emails;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers
{
    public class CallBackRequestWorker : BaseAppWorker
    {
        public CallBackRequestWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public async Task<BaseApiResponse> CreateCallBackRequest(CreateCallBackRequest model)
        {
            var validation = ValidateModel(model);

            if(!validation.IsSucceeded)
            {
                return validation;
            }

            var dateNow = Application.DateTimeProvider.Now;

            var hourBefore = dateNow.AddHours(-1);

            if(!Application.IsDevelopment && await Query<CallBackRequest>().AnyAsync(x => x.IpAddress == model.Ip && x.CreatedOn >= hourBefore))
            {
                return new BaseApiResponse(false, "С вашего Ip адреса уже была отправлена заявка в течение часа, если вы устали ждать связи, мы просим прощения");
            }

            CreateHandled(new CallBackRequest
            {
                EmailOrPhoneNumber = model.EmailOrPhoneNumber,
                IpAddress = model.Ip,
            });


            var res = await TrySaveChangesAndReturnResultAsync("Заявка создана");

            if(res.IsSucceeded)
            {
                Application.JobManager.Enqueue(() => SendEmail(model.EmailOrPhoneNumber));
            }

            return res;
        }

        public static void SendEmail(string emailOrPhoneNumber)
        {
            new CrocoTransactionHandler(() => new SystemCrocoAmbientContext()).ExecuteAndCloseTransactionSafe(amb =>
            {
                new InnerSmtpEmailSender(amb).SendEmail(new SendEmailModel
                {
                    Body = $"Создана заявка на перезвон '{emailOrPhoneNumber}'",
                    Email = "dimaserd84@gmail.com",
                    Subject = "Перезвони"
                }, x => x);

                return Task.CompletedTask;
            });
        }
    }
}