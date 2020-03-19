﻿using Clt.Contract.Models.Account;
using Clt.Contract.Models.Common;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clt.Logic.Core.Workers
{
    public class PasswordChanger<TUser> : BaseCltWorker where TUser : IdentityUser, new()
    {
        public PasswordChanger(ICrocoAmbientContext context) : base(context)
        {
        }

        public async Task<BaseApiResponse> ChangePasswordAsync(ChangeUserPasswordModel model, UserManager<TUser> userManager, SignInManager<TUser> signInManager)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы не авторизованы");
            }

            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (model.NewPassword == model.OldPassword)
            {
                return new BaseApiResponse(false, "Новый и старый пароль совпадют");
            }

            var user = await userManager.FindByIdAsync(UserId);

            if (user == null)
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

        public async Task<BaseApiResponse> ChangePasswordByAdminAsync(ResetPasswordByAdminModel model, UserManager<TUser> userManager, Func<string, Task<ApplicationUserBaseModel>> getUserByIdFunc)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var userDto = await getUserByIdFunc(user.Id);

            var result = UserRightsWorker.HasRightToEditUser(userDto, User, RolesSetting);

            if (!result.IsSucceeded)
            {
                return result;
            }

            return await ChangePasswordBaseAsync(model, userManager);
        }

        /// <summary>
        /// Данный метод не может быть вынесен в API (Базовый метод)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> ChangePasswordBaseAsync(ResetPasswordByAdminModel model, UserManager<TUser> userManager)
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
    }
}