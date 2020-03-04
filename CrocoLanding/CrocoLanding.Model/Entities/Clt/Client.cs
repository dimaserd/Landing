using System;
using System.ComponentModel.DataAnnotations.Schema;
using Zoo.Clt.Entities;

namespace CrocoLanding.Model.Entities.Clt
{
    [Table(nameof(Client), Schema = Schemas.CltSchema)]
    public class Client : WebApplicationUser
    {
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Пол (Null - не указано, 0 - женский, 1 - мужской)
        /// </summary>
        public bool? Sex { get; set; }

        /// <summary>
        /// Баланс пользователя
        /// </summary>
        public decimal Balance { get; set; }

        public bool DeActivated { get; set; }

        public string ObjectJson { get; set; }

        public string PhoneNumber { get; set; }
    }
}