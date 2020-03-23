using System.ComponentModel.DataAnnotations;

namespace Clt.Contract.Models.Common
{
    public class UserWithNameAndEmailAvatarModel : UserIdModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Идентификатор аватара")]
        public int? AvatarFileId { get; set; }
    }
}