using Ecc.Model.Enumerations;

namespace Ecc.Logic.Models.Interactions
{
    /// <summary>
    /// Модель для обновления статусов взаимодействий
    /// </summary>
    public class UpdateInteractionStatus
    {
        /// <summary>
        /// Идентификатор взаимодействия
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Новый статус взаимодейстия
        /// </summary>
        public InteractionStatus Status { get; set; }

        /// <summary>
        /// Описание статуса взаимодействия (может содержать Exception StackTrace)
        /// </summary>
        public string StatusDescription { get; set; }
    }
}