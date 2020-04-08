﻿using Croco.Core.Abstractions.Models;
using Croco.Core.EventSourcing.Implementations;
using Croco.Core.Implementations.TransactionHandlers;
using Croco.Core.Utils;
using Ecc.Contract.Models;
using Ecc.Contract.Models.EmailGroup;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Core.Workers;
using Ecc.Model.Entities.Email;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Handlers
{
    public class SendMailsForEmailGroupMessageHandler : CrocoMessageHandler<SendMailsForEmailGroup>
    {
        IEccPixelUrlProvider UrlProvider { get; }
        IEccTextFunctionsProvider TextFunctionsProvider { get; }

        const int CountInPack = 100;

        public SendMailsForEmailGroupMessageHandler(IEccPixelUrlProvider urlProvider, IEccTextFunctionsProvider textFunctionsProvider)
        {
            UrlProvider = urlProvider;
            TextFunctionsProvider = textFunctionsProvider;
        }


        public async Task<BaseApiResponse> StartEmailDistribution(SendMailsForEmailGroup model)
        {
            var eamilsInGroupSafeValue = await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
            {
                IQueryable<EmailInEmailGroupRelation> initQuery = amb.RepositoryFactory.Query<EmailInEmailGroupRelation>()
                    .Where(x => x.EmailGroupId == model.EmailGroupId)
                    .OrderBy(x => x.Email);
                
                return initQuery.Select(x => x.Email).ToListAsync();
            });
                
            if (!eamilsInGroupSafeValue.IsSucceeded)
            {
                throw new Exception("Не удалось получить список эмейлов из группы");
            }

            var emails = eamilsInGroupSafeValue.Value;

            var count = 0;

            var messageDistribution = Guid.NewGuid().ToString();

            await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
            {
                amb.RepositoryFactory.GetRepository<MessageDistribution>().CreateHandled(new MessageDistribution
                {
                    Id = messageDistribution,
                    Type = "SendMailsByEmailGroup",
                    Data = Tool.JsonConverter.Serialize(model)
                });

                return amb.RepositoryFactory.SaveChangesAsync();
            });

            while (count < emails.Count)
            {
                await CrocoTransactionHandler.System.ExecuteAndCloseTransactionSafe(amb =>
                {
                    var sender = new EmailDelayedSender(amb, UrlProvider, TextFunctionsProvider);

                    return sender.SendEmails(emails.Skip(count).Take(CountInPack).Select(x => new SendMailMessage
                    {
                        Email = x,
                        Body = model.Body,
                        Subject = model.Subject,
                        MessageDistributionId = messageDistribution,
                        AttachmentFileIds = model.AttachmentFileIds
                    }));
                });

                count += CountInPack;
            }

            return new BaseApiResponse(true, "Ok");
        }

        public override Task HandleMessage(SendMailsForEmailGroup model)
        {
            return StartEmailDistribution(model);
        }
    }
}