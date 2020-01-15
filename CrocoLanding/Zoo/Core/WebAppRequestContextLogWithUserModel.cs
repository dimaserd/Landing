using Croco.Core.Logic.Models.Users;

namespace Zoo.Core
{
    public class WebAppRequestContextLogWithUserModel
    {
        public WebAppRequestContextLogModel Log { get; set; }

        public UserNameAndEmailModel User { get; set; }
    }
}