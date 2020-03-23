using Cmn.Enums;
using CrocoLanding.Model.Entities.Clt.Default;
using Microsoft.AspNetCore.Identity;

namespace Clt.Logic.Extensions
{
    public static class UserManagerExtensions
    {
        /// <summary>
        /// Добавляет право пользователю
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="user"></param>
        /// <param name="userRight"></param>
        /// <returns></returns>
        public static IdentityResult AddRight(this UserManager<ApplicationUser> userManager, ApplicationUser user, UserRight userRight)
        {
            return userManager.AddToRoleAsync(user, userRight.ToString()).GetAwaiter().GetResult();
        }
    }
}