using Cmn.Enums;

namespace Clt.Contract.Models.Users
{
    public class UserIdAndRole
    {
        public string UserId { get; set; }
        public UserRight Role { get; set; }
    }
}