using Ecc.Contract.Models.Emails;
using Ecc.Implementation.Settings;
using Ecc.Logic.Abstractions;

namespace Ecc.Implementation.Services
{
    public class AppEmailSenderProvider : IEmailSenderProvider
    {
        public AppEmailSenderProvider(SendGridEmailSettings settings)
        {
            Settings = settings;
        }

        SendGridEmailSettings Settings { get; }

        public IEmailSender GetEmailSender(GetEmailSenderOptions options)
        {
            return new SendGridEmailSender(Settings);
        }
    }
}