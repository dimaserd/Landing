using Croco.Core.Abstractions.Data.Entities.HaveId;
using Croco.Core.Model.Entities.Application;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoo.Ecc.Entities.Chats
{
    public class ChatMessageAttachment<TChatMessage, TFile> : IHaveStringId
        where TChatMessage : class
        where TFile : DbFileIntId
    {
        public string Id { get; set; }

        public string ChatMessageId { get; set; }

        [ForeignKey(nameof(ChatMessageId))]
        public virtual TChatMessage ChatMessage { get; set; }

        public int FileId { get; set; }

        [ForeignKey(nameof(FileId))]
        public virtual TFile File { get; set; }
    }
}