using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Model.Models;
using Croco.Core.Model.Abstractions.Entity;
using Croco.Core.Abstractions.Data.Entities.HaveId;

namespace Zoo.Ecc.Entities
{
    public class Interaction<TUser, TAttachment, TInteractionStatusLog, TInteractionType> : AuditableEntityBase, IHaveStringId
        where TUser : class, ICrocoUser 
        where TAttachment : class
        where TInteractionStatusLog : class
        where TInteractionType : Enum
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public TInteractionType Type { get; set; }

        
        public string MessageText { get; set; }

        public string TitleText { get; set; }

        public string MaskItemsJson { get; set; }

        /// <summary>
        /// Отправить немедленно
        /// </summary>
        public bool SendNow { get; set; }

        /// <summary>
        /// Отправить в определенное время
        /// </summary>
        public DateTime? SendOn { get; set; }

        /// <summary>
        /// Сообщение было отправлено в данную дату
        /// </summary>
        public DateTime? SentOn { get; set; }

        /// <summary>
        /// Сообщение было доставлено в данную дату
        /// </summary>
        public DateTime DeliveredOn { get; set; }

        /// <summary>
        /// Сообщение было прочитано в данную дату
        /// </summary>
        public DateTime? ReadOn { get; set; }

        /// <summary>
        /// Идентификатор пользователя, которому нужно отправить сообщение
        /// </summary>
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual TUser User { get; set; }

        public virtual ICollection<TInteractionStatusLog> Statuses { get; set; }

        public virtual ICollection<TAttachment> Attachments { get; set; }
    }
}