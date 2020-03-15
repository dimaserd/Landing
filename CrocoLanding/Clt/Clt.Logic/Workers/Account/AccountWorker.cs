using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Clt.Contract.Events;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Clt.Contract.Models.Account;
using Microsoft.AspNetCore.Identity;
using Clt.Logic.Workers.Users;
using Cmn.Enums;
using Clt.Logic.Extensions;
using Ecc.Model.Entities.External;
using CrocoLanding.Model.Entities.Clt.Default;
using CrocoLanding.Logic.Settings.Statics;
using CrocoLanding.Model.Entities.Clt;
using CrocoLanding.Logic;
using Clt.Logic.Settings;

namespace Clt.Logic.Workers.Account
{
    public class AccountManager : BaseAppWorker
    {
        /// <summary>
        /// Создается пользователь Root в системе и ему присваиваются все необходимые права
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> InitAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            await RoleFromEnumCreator.CreateRolesAsync<UserRight, ApplicationRole>(roleManager);
            
            var maybeRoot = await userManager.FindByEmailAsync(RightsSettings.RootEmail);

            if (maybeRoot == null)
            {
                maybeRoot = new ApplicationUser
                {
                    Email = RightsSettings.RootEmail,
                    EmailConfirmed = true,
                    UserName = RightsSettings.RootEmail,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                await userManager.CreateAsync(maybeRoot, RightsSettings.RootPassword);

                CreateHandled(new Client
                {
                    Id = maybeRoot.Id,
                    Email = RightsSettings.RootEmail,
                    Name = RightsSettings.RootEmail
                });

                CreateHandled(new EccUser
                {
                    Id = maybeRoot.Id
                });

                await SaveChangesAsync();
            }

            foreach (UserRight right in Enum.GetValues(typeof(UserRight)))
            {
                userManager.AddRight(maybeRoot, right);
            }

            return new BaseApiResponse(true, "Пользователь root создан");
        }

        #region Методы изменения


        #endregion

        #region Методы восстановления пароля

        public async Task<BaseApiResponse> UserForgotPasswordByEmailHandlerAsync(ForgotPasswordModel model, UserManager<ApplicationUser> userManager)
        {
            if(model == null)
            {
                return new BaseApiResponse(false, "Вы подали пустую модель");
            }

            if(IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы авторизованы в системе");
            }

            var searcher = new UserSearcher(AmbientContext);
            
            var user = await searcher.GetUserByEmailAsync(model.Email);

            if(user == null)
            {
                return new BaseApiResponse(false, $"Пользователь не найден по указанному электронному адресу {model.Email}");
            }

            return await UserForgotPasswordNotificateHandlerAsync(user.ToEntity(), userManager);
        }

        public async Task<BaseApiResponse> UserForgotPasswordByPhoneHandlerAsync(ForgotPasswordModelByPhone model, UserManager<ApplicationUser> userManager)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы авторизованы в системе");
            }
            
            var searcher = new UserSearcher(AmbientContext);

            var user = await searcher.GetUserByPhoneNumberAsync(model.PhoneNumber);

            return await UserForgotPasswordNotificateHandlerAsync(user.ToEntity(), userManager);
        }

        private async Task<BaseApiResponse> UserForgotPasswordNotificateHandlerAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var accountSettings = GetSetting<AccountSettingsModel>();

            if (user == null || (!user.EmailConfirmed && accountSettings.ShouldUsersConfirmEmail))
            {
                // Не показывать, что пользователь не существует или не подтвержден
                return new BaseApiResponse(false, "Пользователь не существует или его Email не подтверждён");
            }

            await userManager.UpdateSecurityStampAsync(user);

            // Отправка сообщения электронной почты с этой ссылкой
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            
            await PublishMessageAsync(new ClientStartedRestoringPasswordEvent
            {
                Code = HttpUtility.UrlEncode(code),
                UserId = user.Id
            });

            return new BaseApiResponse(true, "Ok");
        }

        public async Task<BaseApiResponse> ChangePasswordByToken(ChangePasswordByToken model, UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if(user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            var resp = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if(!resp.Succeeded)
            {
                return new BaseApiResponse(false, resp.Errors.First().Description);
            }

            await PublishMessageAsync(new ClientChangedPassword
            {
                ClientId = user.Id
            });

            return new BaseApiResponse(true, "Ваш пароль был изменён");
        }
        #endregion


        public AccountManager(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}