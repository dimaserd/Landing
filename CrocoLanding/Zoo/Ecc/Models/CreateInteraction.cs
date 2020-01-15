using System;
using System.Collections.Generic;

namespace Zoo.Ecc.Models
{
    public class CreateInteraction<TInteractionType> where TInteractionType : Enum
    {
        public TInteractionType Type { get; set; }

        public string MessageText { get; set; }

        public string TitleText { get; set; }

        public string MaskItemsJson { get; set; }

        /// <summary>
        /// Отправить немедленно
        /// </summary>
        public bool SendNow { get; set; }

        /// <summary>
        /// Отправить в данное время
        /// </summary>
        public DateTime? SendOn { get; set; }

        public string UserId { get; set; }

        public List<int> AttachmentFileIds { get; set; }
    }
}
