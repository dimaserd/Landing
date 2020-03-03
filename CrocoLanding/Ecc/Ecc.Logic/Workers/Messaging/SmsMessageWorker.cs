using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Search.Models;
using Ecc.Logic.Resources;
using Ecc.Logic.Models.Messaging;
using Ecc.Logic.Models.Sms;
using Ecc.Model.Entities.External;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecc.Model.Consts;

namespace Ecc.Logic.Workers.Messaging
{
    public class SmsMessageWorker : ApplicationInteractionWorker
    {
        public SmsMessageWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public async Task<BaseApiResponse> SendSms(SendSmsToClient model)
        {
            if(!IsUserAdmin())
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            var phoneAndId = await Query<EccUser>().Select(x => new { x.PhoneNumber, x.Id }).FirstOrDefaultAsync(x => x.Id == model.ClientId);

            if (phoneAndId == null)
            {
                return new BaseApiResponse(false, "Клиент не найден по указанному идентификатору");
            }

            var toMes = ToSmsMessage(model, phoneAndId.PhoneNumber);

            GetRepository<SmsMessageInteraction>().CreateHandled(toMes.Item1);
            GetRepository<InteractionStatusLog>().CreateHandled(toMes.Item2);

            return await TrySaveChangesAndReturnResultAsync("Sms-сообщение добавлено в очередь");
        }

        public Task<GetListResult<SmsMessageModel>> GetClientSmsMessages(GetClientInteractions model)
        {
            var initQuery = Query<SmsMessageInteraction>();

            if(!string.IsNullOrWhiteSpace(model.ClientId))
            {
                initQuery = initQuery.Where(x => x.UserId == model.ClientId);
            }

            return GetListResult<SmsMessageModel>.GetAsync(model, GetQueryWithStatus(initQuery).OrderByDescending(x => x.Interaction.CreatedOn), SmsMessageModel.SelectExpression);
        }

        public (SmsMessageInteraction, InteractionStatusLog, List<InteractionAttachment>) ToSmsMessage(SendSmsToClient message, string phoneNumber)
        {
            var id = Guid.NewGuid().ToString();

            var attachments = message.AttachmentFileIds?.Select(x => new InteractionAttachment
            {
                FileId = x,
                InteractionId = id,
            }).ToList();

            return (new SmsMessageInteraction
            {
                Id = id,
                TitleText = null,
                MessageText = message.Message,
                SendNow = true,
                UserId = message.ClientId,
                Type = EccConsts.SmsType,
                Attachments = attachments,
                PhoneNumber = phoneNumber,
            },
            new InteractionStatusLog
            {
                InteractionId = id,
                Status = InteractionStatus.Created,
                StartedOn = Application.DateTimeProvider.Now
            },
            attachments);
        }
    }
}