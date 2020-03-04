using System;
using System.Threading.Tasks;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using CrocoShop.Logic.Extensions;
using Clt.Contract.Models.Account;
using CrocoShop.Logic.Workers.Base;
using Clt.Logic.Workers.Users;
using Clt.Logic.Models.Account;
using Clt.Logic.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clt.Contract.Models.Common;
using Cmn.Enums;
using CrocoShop.Logic.Settings.Statics;
using CrocoShop.Logic.Settings;
using CrocoShop.Model.Entities.Clt.Default;
using CrocoShop.Model.Entities.Clt;

namespace Clt.Logic.Workers.Account
{
    public class AccountLoginWorker : BaseWorker
    {
        public async Task<BaseApiResponse<LoginResultModel>> LoginAsync(LoginModel model, SignInManager<ApplicationUser> signInManager)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<LoginResultModel>(validation);
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Вы уже авторизованы в системе", new LoginResultModel { Result = LoginResult.AlreadyAuthenticated });
            }

            model.RememberMe = true;

            var result = false;

            var user = await signInManager.UserManager.FindByEmailAsync(model.Email);

            var client = await Query<Client>()
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null || client == null)
            {
                if(user != null && client == null)
                {
                    Logger.LogException(new Exception($"There is user without client {user.Id}"));
                }

                return new BaseApiResponse<LoginResultModel>(false, "Неудачная попытка входа", new LoginResultModel { Result = LoginResult.UnSuccessfulAttempt });
            }

            if (client.DeActivated)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Ваша учетная запись деактивирована", new LoginResultModel { Result = LoginResult.UserDeactivated });
            }

            var accountSettings = GetSetting<AccountSettingsModel>();

            //если логинирование не разрешено для пользователей которые не подтвердили Email и у пользователя Email не потверждён
            if (user.Email != RightsSettings.RootEmail && !user.EmailConfirmed && !accountSettings.IsLoginEnabledForUsersWhoDidNotConfirmEmail)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Ваш Email не подтверждён", new LoginResultModel { Result = LoginResult.EmailNotConfirmed });
            }

            try
            {
                var userWorker = new UserWorker(AmbientContext);

                //проверяю пароль
                var passCheckResult = await userWorker.CheckUserNameAndPasswordAsync(user.Id, user.UserName, model.Password);

                //если пароль не подходит выдаю ответ
                if (!passCheckResult.IsSucceeded)
                {
                    return new BaseApiResponse<LoginResultModel>(false, "Неудачная попытка входа", new LoginResultModel { Result = LoginResult.UnSuccessfulAttempt, TokenId = null });
                }
                
                if (user.Email == RightsSettings.RootEmail) //root входит без подтверждений
                {
                    await signInManager.SignInAsync(user, model.RememberMe);

                    return new BaseApiResponse<LoginResultModel>(true, "Вы успешно авторизованы", new LoginResultModel { Result = LoginResult.SuccessfulLogin });
                }

                if (accountSettings.ConfirmLogin == AccountSettingsModel.ConfirmLoginType.None) //не логинить пользователя если нужно подтвержать логин
                {
                    await signInManager.SignInAsync(user, model.RememberMe);

                    result = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                return new BaseApiResponse<LoginResultModel>(false, ex.Message);
            }
            
            if (result)
            {   
                return new BaseApiResponse<LoginResultModel>(true, "Авторизация прошла успешно", new LoginResultModel { Result = LoginResult.SuccessfulLogin, TokenId = null });
            }

            return new BaseApiResponse<LoginResultModel>(false, "Неудачная попытка входа", new LoginResultModel { Result = LoginResult.UnSuccessfulAttempt, TokenId = null });
        }

        public async Task<BaseApiResponse<LoginResultModel>> LoginByPhoneNumberAsync(LoginByPhoneNumberModel model, SignInManager<ApplicationUser> signInManager)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return new BaseApiResponse<LoginResultModel>(validation);
            }

            var searcher = new UserSearcher(AmbientContext);

            var user = await searcher.GetUserByPhoneNumberAsync(model.PhoneNumber);

            if (user == null)
            {
                return new BaseApiResponse<LoginResultModel>(false, "Пользователь не найден по указанному номеру телефона");
            }

            return await LoginAsync(LoginModel.GetModel(model, user.Email), signInManager);
        }

        public async Task<BaseApiResponse> LoginAsUserAsync(UserIdModel model, SignInManager<ApplicationUser> signInManager)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }
            
            if(!User.HasRight(UserRight.Root))
            {
                return new BaseApiResponse(false, "У вас недостаточно прав для логинирования за другого пользователя");
            }

            var user = await signInManager.UserManager.FindByIdAsync(model.Id);

            if(user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по укаанному идентификатору");
            }

            await signInManager.SignInAsync(user, true);

            return new BaseApiResponse(true, $"Вы залогинены как {user.Email}");
        }

        /// <summary>
        /// Разлогинивание в системе
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticationManager"></param>
        /// <returns></returns>
        public BaseApiResponse LogOut(IApplicationAuthenticationManager authenticationManager)
        {
            if(!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы и так не авторизованы");
            }

            authenticationManager.SignOut();

            return new BaseApiResponse(true, "Вы успешно разлогинены в системе");
        }

        public AccountLoginWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}