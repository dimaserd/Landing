using Croco.Core.Abstractions.Data.Entities.HaveId;
using Croco.Core.Model.Abstractions.Entity;
using Croco.Core.Model.Entities.Application;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoo.Ecc.Entities.Chats
{
    public class ChatMessage<TChat, TUser, TChatMessageAttachment>
        where TChat : class, IHaveIntId
        where TUser : class, ICrocoUser
        where TChatMessageAttachment : class
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public long SentOnUtcTicks { get; set; }

        public int ChatId { get; set; }

        [ForeignKey(nameof(ChatId))]
        public virtual TChat Chat { get; set; }

        public string SenderUserId { get; set; }

        [ForeignKey(nameof(SenderUserId))]
        public virtual TUser SenderUser { get; set; }

        /// <summary>
        /// Вложения
        /// </summary>
        public virtual ICollection<TChatMessageAttachment> Attachments { get; set; }
    }
}