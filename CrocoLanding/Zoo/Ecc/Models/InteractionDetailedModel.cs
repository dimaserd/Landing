using Croco.Core.Logic.Models.Users;
using System;
using System.Collections.Generic;

namespace Zoo.Ecc.Models
{
    public class InteractionDetailedModel<TInteractionType> where TInteractionType : Enum
    {
        public string Id { get; set; }

        public TInteractionType Type { get; set; }

        public List<InteractionStatusLogModel> Statuses { get; set; }

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

        public UserNameAndEmailModel User { get; set; }

        public List<int> AttachmentFileIds { get; set; }
    }
}
