using Croco.Core.Logic.Models.Users;

namespace Ecc.Contract.Models.Chat
{
    public class UserInChatModel
    {
        public UserNameAndEmailModel User { get; set; }

        public long LastVisitUtcTicks { get; set; }
    }
}