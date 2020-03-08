using Ecc.Logic.Abstractions;

namespace Ecc.Implementation.Services
{
    public class AppEccPixelUrlProvider : IEccPixelUrlProvider
    {
        public AppEccPixelUrlProvider(string applicationUrl)
        {
            ApplicationUrl = applicationUrl;
        }

        string ApplicationUrl { get; }

        /// <summary>
        /// Получить адрес для установки пикселя для определения прочитанности писем
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string GetPixelEmailUrl(string interactionId)
        {
            return $"{ApplicationUrl}/Img/{interactionId}.jpg";
        }
    }
}