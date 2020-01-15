using Croco.Core.Abstractions.Settings;

namespace CrocoLanding.Controllers
{
    public class EmailSettingsModel : ICommonSetting<EmailSettingsModel>
    {
        public string FromAddress { get; set; }
        public bool IsBodyHtml { get; set; }
        public string SmtpClientString { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public EmailSettingsModel GetDefault()
        {
            return new EmailSettingsModel
            {
                FromAddress = "emailer96@mail.ru",
                IsBodyHtml = true,
                SmtpClientString = "smtp.mail.ru",
                SmtpPort = 587,
                UserName = "emailer96@mail.ru",
                Password = "ADGwxec135"
            };
        }
    }
}