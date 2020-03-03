using Ecc.Model.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.Email
{
    [Table(nameof(EmailTemplate), Schema = Schemas.EccSchema)]
    public class EmailTemplate
    {
        public string Id { get; set; }

        [Display(Name = "Тип сообщения")]
        public EmailTemplateType Type { get; set; }

        public bool IsActive { get; set; }

        public string CustomEmailType { get; set; }

        /// <summary>
        /// Данный джаваскрипт должен описывать две функции GetEmailBody() и GetEmailSubject
        /// </summary>
        [Display(Name = "Скрипт сообщения")]
        public string JsScript { get; set; }

        public bool IsJsScripted { get; set; }
    }
}