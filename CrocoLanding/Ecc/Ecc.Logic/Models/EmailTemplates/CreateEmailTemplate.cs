using System.ComponentModel.DataAnnotations;
using Ecc.Model.Enumerations;

namespace Ecc.Logic.Models.EmailTemplates
{
    public class CreateEmailTemplate
    {
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