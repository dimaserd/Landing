using Croco.Core.Abstractions.Data.Entities.HaveId;
using Croco.Core.Model.Abstractions.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoo.Ecc.Entities.Chats
{
    public class ChatUserRelation<TChat, TUser>
        where TChat : class, IHaveIntId
        where TUser : class, ICrocoUser
    {
        public int ChatId { get; set; }

        [ForeignKey(nameof(ChatId))]
        public virtual TChat Chat { get; set; }

        public bool IsChatCreator { get; set; }

        public long LastVisitUtcTicks { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual TUser User { get; set; }

        public static void OnModelCreating<TChatUserRelation>(ModelBuilder modelBuilder) where TChatUserRelation : ChatUserRelation<TChat, TUser>
        {
            modelBuilder.Entity<TChatUserRelation>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }
    }
}