using System.ComponentModel.DataAnnotations;

namespace Ecc.Logic.Models.MailDistributions
{
    public class MailDistributionEdit : MailDistributionIdModel
    {
        [Required(ErrorMessage = "Название рассылки не может быть пустым")]
        public string Name { get; set; }

        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Тело сообщения
        /// </summary>
        public string Body { get; set; }

        public bool SendToEveryUser { get; set; }
    }
}