using Croco.WebApplication.Application;
using Ecc.Contract.Models.Emails;
using Ecc.Contract.Settings;
using Ecc.Logic.Abstractions;
using Ecc.Logic.Workers.Emails;
using Microsoft.AspNetCore.StaticFiles;

namespace CrocoLanding.Logic
{
    public class LandingWebApplication : CrocoWebApplication, IEccApplication
    {
        public LandingWebApplication(CrocoWebApplicationOptions options) : base(options)
        {
        }

        public static string GetMimeMapping(string fileName)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);

            return contentType ?? "application/octet-stream";
        }

        public static bool IsImage(string fileName)
        {
            return GetMimeMapping(fileName).StartsWith("image/");
        }

        /// <summary>
        /// Получить адрес для установки пикселя для определения прочитанности писем
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string GetPixelEmailUrl(string emailId)
        {
            return $"{ApplicationUrl}/Img/{emailId}.jpg";
        }

        public IEmailSender GetEmailSender(GetEmailSenderOptions options)
        {
            var settings = SettingsFactory.GetSetting<EmailSettingsModel>();

            return new InnerSmtpEmailSender(options.AmbientContext, settings, options.Attachments);
        }


        public bool IsDevelopment { get; set; }
    }
}