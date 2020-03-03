using Ecc.Model.Entities.Interactions;
using Ecc.Model.Enumerations;
using System;
using System.Linq.Expressions;

namespace Ecc.Logic.Models.Sms
{
    public class SmsMessageModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string Header { get; set; }

        public DateTime? ReadOn { get; set; }

        public DateTime? SentOn { get; set; }

        public string PhoneNumber { get; set; }

        public InteractionStatus Status { get; set; }

        public static Expression<Func<ApplicationInteractionWithStatus<SmsMessageInteraction>, SmsMessageModel>> SelectExpression = x => new SmsMessageModel
        {
            Id = x.Interaction.Id,
            Body = x.Interaction.MessageText,
            Header = x.Interaction.TitleText,
            ReadOn = x.Interaction.ReadOn,
            SentOn = x.Interaction.SentOn,
            PhoneNumber = x.Interaction.PhoneNumber,
            Status = x.Status
        };
    }
}