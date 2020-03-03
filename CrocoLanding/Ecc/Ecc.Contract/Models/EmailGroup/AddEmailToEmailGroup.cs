using System.ComponentModel.DataAnnotations;

namespace Ecc.Contract.Models.EmailGroup
{
    public class AddEmailToEmailGroup
    {
        public string EmailGroupId { get; set; }

        [EmailAddress(ErrorMessage = "Нужно указать Email")]
        public string Email { get; set; }
    }
}