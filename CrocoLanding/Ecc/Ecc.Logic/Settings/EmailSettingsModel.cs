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
                FromAddress = "info@crocosoft.ru",
                IsBodyHtml = true,
                SmtpClientString = "smtp.yandex.ru",
                SmtpPort = 25,
                UserName = "info@crocosoft.ru",
                Password = "Cthl.rjd"
            };
        }
    }
}