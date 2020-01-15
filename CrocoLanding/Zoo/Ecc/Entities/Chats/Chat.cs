using Croco.Core.Abstractions.Data.Entities.HaveId;
using System.Collections.Generic;

namespace Zoo.Ecc.Entities.Chats
{
    public class Chat<TChatMessage, TChatUserRelation> : IHaveIntId
        where TChatUserRelation : class
        where TChatMessage : class
    {
        public int Id { get; set; }

        public bool IsDialog { get; set; }

        public string ChatName { get; set; }

        public virtual ICollection<TChatMessage> Messages { get; set; }

        public virtual ICollection<TChatUserRelation> UserRelations { get; set; }
    }
}