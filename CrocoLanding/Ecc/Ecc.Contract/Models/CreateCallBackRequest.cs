using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models
{
    public class CreateCallBackRequest
    {
        public string Ip { get; set; }

        [Required(ErrorMessage = "Необходимо указать адрес электронной почты или номер телефона")]
        public string EmailOrPhoneNumber { get; set; }
    }
}