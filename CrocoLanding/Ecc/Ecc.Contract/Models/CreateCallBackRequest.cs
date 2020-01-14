namespace Ecc.Contract.Models
{
    public class CreateCallBackRequest
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public string Uid { get; set; }

        public string EmailOrPhoneNumber { get; set; }
    }
}