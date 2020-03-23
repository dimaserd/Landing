using System;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Croco.Core.Abstractions;
using System.Linq.Expressions;
using CrocoLanding.Logic;
using CrocoLanding.Model.Entities.Clt.Default;
using Clt.Contract.Models.Account;

namespace Clt.Logic.Workers.Users
{
    public class UserWorker : BaseAppWorker
    {
        
        #region Изменение пароля
        
        /// <summary>
        /// Данный метод не может быть вынесен в API (Базовый метод)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> ChangePasswordBaseAsync(ResetPasswordByAdminModel model, UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetResult = await userManager.ResetPasswordAsync(user, code, model.Password);

            if (!resetResult.Succeeded)
            {
                return new BaseApiResponse(resetResult.Succeeded, resetResult.Errors.First().Description);
            }

            return new BaseApiResponse(true, $"Вы изменили пароль для пользователя {user.Email}");
        }
        #endregion

        public async Task<BaseApiResponse> CheckUserNameAndPasswordAsync(string userId, string userName, string pass)
        {
            var user = await Query<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.Id == userId);

            var passHasher = new PasswordHasher<ApplicationUser>();
            
            var t = passHasher.VerifyHashedPassword(user, user.PasswordHash, pass) != PasswordVerificationResult.Failed && user.UserName == userName;

            return new BaseApiResponse(t, "");
        }

        public async Task GenericDelete<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : class
        {
            DeleteHandled(await Query<TEntity>().Where(whereExpression).ToListAsync());
        }

        
        public UserWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}