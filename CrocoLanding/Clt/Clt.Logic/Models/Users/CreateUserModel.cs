using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clt.Contract.Models.Account;
using Cmn.Enums;

namespace Clt.Logic.Models.Users
{
    public class CreateUserModel : RegisterModel
    {
        [Display(Name = "Права пользователя")]
        public List<UserRight> Rights { get; set; }
    }
}