using Croco.Core.Abstractions.Settings;

namespace Ecc.Implementation.Settings
{
    public class SendGridEmailSettings : ICommonSetting<SendGridEmailSettings>
    {
        public string ApiKey { get; set; }

        public string FromAddress { get; set; }

        public SendGridEmailSettings GetDefault()
        {
            return new SendGridEmailSettings
            {
                FromAddress = "info@crocosoft.ru",
                ApiKey = "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"
            };
        }
    }
}