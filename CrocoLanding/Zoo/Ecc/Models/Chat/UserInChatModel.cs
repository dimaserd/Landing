using Croco.Core.Logic.Models.Users;

namespace Zoo.Ecc.Models.Chat
{
    public class UserInChatModel
    {
        public UserNameAndEmailModel User { get; set; }

        public long LastVisitUtcTicks { get; set; }
    }
}