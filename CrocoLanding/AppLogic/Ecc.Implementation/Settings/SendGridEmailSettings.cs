namespace Ecc.Implementation.Settings
{
    public class SendGridEmailSettings
    {
        public string ApiKey { get; set; }

        public string FromAddress { get; set; }

        public SendGridEmailSettings()
        {
            FromAddress = "info@crocosoft.ru";
            ApiKey = "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";
        }
    }
}