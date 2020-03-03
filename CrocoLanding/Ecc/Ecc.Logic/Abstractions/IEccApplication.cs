using Ecc.Contract.Models.Emails;

namespace Ecc.Logic.Abstractions
{
    /// <summary>
    /// Абстракция для приложения с внешними рассылками
    /// </summary>
    public interface IEccApplication
    {
        /// <summary>
        /// Получить рассыльщик для электронной почты
        /// </summary>
        /// <returns></returns>
        public IEmailSender GetEmailSender(GetEmailSenderOptions options);
    }
}