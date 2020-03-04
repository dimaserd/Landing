using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cmn.Enums;

namespace Clt.Contract.Models.Common
{
    public class ApplicationUserBaseModel : UserWithNameAndEmailAvatarModel
    {
        [Description("Email подтвержден")]
        [Display(Name = "Email подтвержден")]
        public bool EmailConfirmed { get; set; }

        [Description("Номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Description("Номер телефона подтвержден")]
        [Display(Name = "Номер телефона подтвержден")]
        public bool PhoneNumberConfirmed { get; set; }

        [Description("Баланс")]
        [Display(Name = "Баланс")]
        public decimal Balance { get; set; }

        [Description("Пол")]
        [Display(Name = "Пол")]
        public bool? Sex { get; set; }

        [Display(Name = "JSON Объект")]
        public string ObjectJson { get; set; }
        
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        [Display(Name = "Дата рождения")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Деактивирован")]
        public bool DeActivated { get; set; }
        
        [Display(Name = "Права пользователя")]
        public List<string> RoleNames { get; set; }

        
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        public string PasswordHash { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool HasRight(UserRight right)
        {
            if(RoleNames == null)
            {
                throw new ApplicationException("Роли не установлены");
            }

            return RoleNames.Any(x => x == right.ToString());
        }

        public List<UserRight> GetRights()
        {
            return RoleNames.Select(x => 
            {
                UserRight? res = null;

                if(Enum.TryParse(x, out UserRight result))
                {
                    res = result;
                }

                return res;
            }).Where(x => x.HasValue)
            .Select(x => x.Value).ToList();
        }
    }
}