using System;
using Croco.Core.Search.Models;

namespace Clt.Logic.Models.Users
{
    public class UserSearch : GetListSearchModel
    {
        public string Q { get; set; }

        public bool? Deactivated { get; set; }

        public GenericRange<DateTime> RegistrationDate { get; set; }

        public bool SearchSex { get; set; }

        public bool? Sex { get; set; }

        public bool? HasPurchases { get; set; }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        public static UserSearch GetAllUsers => new UserSearch
        {
            Count = null,
            OffSet = 0
        };
    }
}