using Ecc.Model.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ecc.Logic.Models.Notifications
{
    public class UserNotificationModel
    {
        public string Id { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Текст")]
        public string Text { get; set; }

        public string ObjectJson { get; set; }

        public bool Read { get; set; }

        [Display(Name = "Тип уведомления")]
        public UserNotificationType Type { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ReadDate { get; set; }

        public string UserId { get; set; }
    }
}