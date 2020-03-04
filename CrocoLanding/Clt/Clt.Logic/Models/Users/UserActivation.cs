using Clt.Contract.Models.Common;

namespace Clt.Logic.Models.Users
{
    public class UserActivation : UserIdModel
    {
        /// <summary>
        /// Если true, значит пользователь деактивирован в системе
        /// </summary>
        public bool DeActivated { get; set; }
    }
}