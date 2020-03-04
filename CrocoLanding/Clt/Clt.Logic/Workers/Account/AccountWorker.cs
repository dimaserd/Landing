using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Clt.Contract.Events;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Clt.Contract.Models.Account;
using CrocoShop.Logic.Workers.Base;
using Microsoft.AspNetCore.Identity;
using Clt.Logic.Workers.Users;
using Clt.Logic.Abstractions;
using Cmn.Enums;
using Clt.Contract.Models.Common;
using Clt.Logic.Helpers;
using Clt.Logic.Extensions;
using CrocoShop.Logic.Settings.Statics;
using CrocoShop.Logic.Settings;
using CrocoShop.Model.Entities.Clt.Default;
using CrocoShop.Model.Entities.Clt;
using CrocoShop.Logic.Resources;
using Prd.Model.Entities.External;
using Ecc.Model.Entities.External;

namespace Clt.Logic.Workers.Account
{
    public class AccountManager : BaseWorker
    {
        private  async Task CreateRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            var roles = UserHelper.GetAllRights().Select(x => x.ToString()).ToArray();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role, ConcurrencyStamp = Guid.NewGuid().ToString() });
                }
            }

            new BaseApiResponse(true, "Роли созданы");
        }

        /// <summary>
        /// Создается пользователь Root в системе и ему присваиваются все необходимые права
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> InitAsync(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            await CreateRolesAsync(roleManager);
            
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

                CreateHandled(new PrdClient
                {
                    Id = maybeRoot.Id
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

        public BaseApiResponse<ApplicationUserBaseModel> CheckUserChanges(IApplicationAuthenticationManager authenticationManager, SignInManager<ApplicationUser> signInManager)
        {
            if(!IsAuthenticated)
            {
                return new BaseApiResponse<ApplicationUserBaseModel>(true, "Вы не авторизованы в системе", null);
            }

            return new BaseApiResponse<ApplicationUserBaseModel>(true, "", null);

            //TODO Implement CheckUserChanges
        }

        #region Методы изменения

        public async Task<BaseApiResponse> ChangePasswordAsync(ChangeUserPasswordModel model, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAuthorized);
            }

            var validation = ValidateModel(model);

            if(!validation.IsSucceeded)
            {
                return validation;
            }

            if(model.NewPassword == model.OldPassword)
            {
                return new BaseApiResponse(false, "Новый и старый пароль совпадют");
            }

            var user = await userManager.FindByIdAsync(UserId);

            if(user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return new BaseApiResponse(false, "Неправильно указан старый пароль");
            }

            if (user != null)
            {
                await signInManager.SignInAsync(user, true);
            }

            return new BaseApiResponse(true, "Ваш пароль изменен");
        }
        


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