using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clt.Contract.Events;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using CrocoShop.Logic.Services;
using CrocoShop.Logic.Workers.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Cmn.Enums;
using CrocoShop.Logic.Settings;
using Clt.Contract.Models.Account;
using Clt.Contract.Models.Users;
using CrocoShop.Model.Entities.Clt.Default;
using CrocoShop.Model.Entities.Clt;
using Clt.Logic.Extensions;
using Prd.Model.Entities.External;
using Ecc.Model.Entities.External;

namespace Clt.Logic.Workers.Account
{
    public class AccountRegistrationWorker : BaseWorker
    {
        #region Методы регистрации
        public async Task<BaseApiResponse<RegisteredUser>> RegisterAndSignInAsync(RegisterModel model, bool createRandomPassword, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            var result = await RegisterInnerAsync(model, createRandomPassword, userManager);

            if (!result.IsSucceeded)
            {
                return new BaseApiResponse<RegisteredUser>(result);
            }

            try
            {
                var user = result.ResponseObject;

                //авторизация пользователя в системе. через распаковку во внутренную модель
                await signInManager.SignInAsync(user, false);

                return new BaseApiResponse<RegisteredUser>(true, "Регистрация и Авторизация прошла успешно", new RegisteredUser
                {
                    Id = user.Id,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                });
            }
            catch (Exception ex)
            {
                return new BaseApiResponse<RegisteredUser>(ex);
            }
        }

        public async Task<BaseApiResponse<RegisteredUser>> RegisterAsync(RegisterModel model, bool createRandomPass, UserManager<ApplicationUser> userManager)
        {
            var result = await RegisterInnerAsync(model, createRandomPass, userManager);

            if(!result.IsSucceeded)
            {
                return new BaseApiResponse<RegisteredUser>(result);
            }

            return new BaseApiResponse<RegisteredUser>(result.IsSucceeded, result.Message, new RegisteredUser
            {
                Id = result.ResponseObject.Id,
                Email = result.ResponseObject.Email,
                PhoneNumber = result.ResponseObject.PhoneNumber
            });
        }

        private async Task<BaseApiResponse<ApplicationUser>> RegisterInnerAsync(RegisterModel model, bool createRandomPassword, UserManager<ApplicationUser> userManager)
        {
            var accountSettings = GetSetting<AccountSettingsModel>();

            if (!accountSettings.RegistrationEnabled)
            {
                return new BaseApiResponse<ApplicationUser>(false, "В данном приложении запрещена регистрация");
            }

            if (IsAuthenticated)
            {
                return new BaseApiResponse<ApplicationUser>(false, "Вы не можете регистрироваться, так как вы авторизованы в системе");
            }

            if(createRandomPassword)
            {
                model.Password = "testpass";
            }

            var result = await RegisterHelpMethodAsync(model, userManager, new List<UserRight> { UserRight.Customer });

            if (!result.IsSucceeded)
            {
                return new BaseApiResponse<ApplicationUser>(result);
            }

            //Выбрасываем событие о регистрации клиента
            await PublishMessageAsync(new ClientRegisteredEvent
            {
                UserId = result.ResponseObject.Id,
                IsPasswordGenerated = createRandomPassword,
                Password = model.Password
            });

            var user = result.ResponseObject;

            return new BaseApiResponse<ApplicationUser>(true, "Регистрация прошла успешно.", user);
        }

        /// <summary>
        /// Метод регистрирующий пользователя администратором
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userManager"></param>
        /// <param name="userRights"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse<string>> RegisterUserByAdminAsync(RegisterModel model, ApplicationUserManager userManager, List<UserRight> userRights)
        {
            var validation = ValidateModelAndUserIsAdmin(model);
            
            if(!validation.IsSucceeded)
            {
                return new BaseApiResponse<string>(validation);
            }

            var result = await RegisterHelpMethodAsync(model, userManager, userRights);

            if (!result.IsSucceeded)
            {
                return new BaseApiResponse<string>(result);
            }

            //Выбрасываем событие о регистрации клиента
            await PublishMessageAsync(new ClientRegisteredEvent
            {
                UserId = result.ResponseObject.Id
            });

            var user = result.ResponseObject;

            return new BaseApiResponse<string>(true, "Вы успешно зарегистрировали пользователя", user.Id);
        }

        private async Task<BaseApiResponse<ApplicationUser>> RegisterHelpMethodAsync(RegisterModel model, UserManager<ApplicationUser> userManager, List<UserRight> userRights)
        {
            var accountSettings = GetSetting<AccountSettingsModel>();

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = !accountSettings.ShouldUsersConfirmEmail
            };

            var checkResult = await CheckUserAsync(user);

            if (!checkResult.IsSucceeded)
            {
                return new BaseApiResponse<ApplicationUser>(checkResult.IsSucceeded, checkResult.Message);
            }

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return new BaseApiResponse<ApplicationUser>(false, result.Errors.ToList().First().Description);
            }

            userManager.AddRight(user, UserRight.Customer);

            if (userRights != null)
            {
                var rightsThatCantBeAdded = new[]
                {
                    UserRight.SuperAdmin, UserRight.Admin, UserRight.Root, UserRight.Seller
                };

                foreach (var right in userRights.Where(x => !rightsThatCantBeAdded.Contains(x)))
                {
                    userManager.AddRight(user, right);
                }
            }

            //Создается внутренний пользователь
            CreateHandled(new Client
            {
                Id = user.Id,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Patronymic = model.Patronymic,
                PhoneNumber = model.PhoneNumber
            });

            //Создается пользователь для продуктового контекста
            CreateHandled(new PrdClient
            {
                Id = user.Id
            });

            //Создается пользователь для сервиса рассылок
            CreateHandled(new EccUser
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            });

            return await TrySaveChangesAndReturnResultAsync("Пользователь создан", user);
        }

        #endregion

        private async Task<BaseApiResponse> CheckUserAsync(ApplicationUser user)
        {
            if(string.IsNullOrWhiteSpace(user.Email))
            {
                return new BaseApiResponse(false, "Вы не указали адрес электронной почты");
            }

            if(string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                return new BaseApiResponse(false, "Вы не указали номер телефона");
            }

            var query = Query<ApplicationUser>();

            if (await query.AnyAsync(x => x.Email == user.Email))
            {
                return new BaseApiResponse(false, "Пользователь с данным email адресом уже существует");
            }

            if (await query.AnyAsync(x => x.PhoneNumber == user.PhoneNumber))
            {
                return new BaseApiResponse(false, "Пользователь с данным номером телефона уже существует");
            }

            return new BaseApiResponse(true, "");
        }

        public AccountRegistrationWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}