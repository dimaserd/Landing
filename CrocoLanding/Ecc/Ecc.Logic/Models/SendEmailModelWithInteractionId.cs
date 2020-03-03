using Ecc.Contract.Models.Emails;

namespace Ecc.Logic.Models
{
    public class SendEmailModelWithInteractionId
    {
        public string InteractionId { get; set; }

        public SendEmailModel EmailModel { get; set; }
    }
}