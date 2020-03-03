using System.ComponentModel.DataAnnotations;
using Croco.Core.Application;
using Ecc.Model.Enumerations;

namespace Ecc.Logic.Models.Notifications
{
    public class CreateNotification
    {
        [Required(ErrorMessage = "Необходимо указать заголовок уведомления")]
        [Display(Name = "Заголовок уведомления")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Необходимо указать текст уведомления")]
        [Display(Name = "Текст уведомления")]
        public string Text { get; set; }

        public string ObjectJSON { get; set; }

        [Display(Name = "Тип уведомления")]
        public UserNotificationType Type { get; set; }

        [Required(ErrorMessage = "Необходимо указать идентификатор пользователя")]
        public string UserId { get; set; }

        public UserNotificationModel ToNotificationDto(CreateNotification model)
        {
            var now = CrocoApp.Application.DateTimeProvider.Now;

            return new UserNotificationModel
            {
                CreationDate = now,
                UserId = model.UserId,
                Read = false,
                ReadDate = now,
                Title = model.Title,
                Text = model.Text,
                Type = model.Type,
                ObjectJson = model.ObjectJSON,
            };
        }
    }
}