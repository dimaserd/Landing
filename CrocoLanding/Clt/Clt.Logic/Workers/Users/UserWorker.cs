using System;
using System.Linq;
using System.Threading.Tasks;
using Croco.Core.Abstractions.Models;
using CrocoShop.Logic.Resources;
using CrocoShop.Logic.Workers.Base;
using CrocoShop.Logic.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clt.Logic.Models.Users;
using Clt.Logic.Models.Account;
using Croco.Core.Abstractions;
using Cmn.Enums;
using CrocoShop.Logic.Settings.Statics;
using CrocoShop.Model.Entities.Clt.Default;
using CrocoShop.Model.Entities.Clt;
using Croco.Core.Extensions;
using System.Linq.Expressions;
using Prd.Model.Entities;
using Prd.Model.Entities.Purchases;
using Prd.Model.Entities.Warehouse;
using CrocoShop.Logic.Services.Abstractions;
using Ecc.Model.Entities.Interactions;

namespace Clt.Logic.Workers.Users
{
    public class UserWorker : BaseWorker
    {
        
        #region Изменение пароля
        public async Task<BaseApiResponse> ChangePasswordAsync(ResetPasswordByAdminModel model, UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var searcher = new UserSearcher(AmbientContext);

            var userDto = await searcher.GetUserByIdAsync(user.Id);

            var result = UserRightsWorker.HasRightToEditUser(userDto, User);

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


        public async Task<BaseApiResponse> RemoveUserAsync(string userId)
        {
            if(!RightsSettings.UserRemovingEnabled)
            {
                return new BaseApiResponse(false, "В настройках вашего приложения выключена опция удаления пользователей");
            }
            
            if(!User.HasRight(UserRight.Root))
            {
                return new BaseApiResponse(false, "Вы не имеете прав для удаления пользователя");
            }

            var searcher = new UserSearcher(AmbientContext);

            var userToRemove = await searcher.GetUserByIdAsync(userId);

            if(userToRemove == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            if(userToRemove.HasRight(UserRight.Root))
            {
                return new BaseApiResponse(false, "Вы не можете удалить Root пользователя");
            }

            await GenericDelete<Client>(x => x.Id == userId);

            await GenericDelete<CartItem>(x => x.FromCart.CustomerId == userId);
            await GenericDelete<Cart>(x => x.CustomerId == userId);

            await GenericDelete<PurchaseItem>(x => x.FromPurchase.CustomerId == userId);
            await GenericDelete<Purchase>(x => x.CustomerId == userId);
            await GenericDelete<ProductItemWayBill>(x => x.PurchaseItem.FromPurchase.CustomerId == userId);

            await GenericDelete<CustomerProductLike>(x => x.CustomerId == userId);
            await GenericDelete<CustomerProductVisit>(x => x.CustomerId == userId);
            await GenericDelete<ProductItemWayBill>(x => x.UserId == userId);
            await GenericDelete<UserNotificationInteraction>(x => x.UserId == userId);
            await GenericDelete<MailMessageInteraction>(x => x.UserId == userId);
            await GenericDelete<SmsMessageInteraction>(x => x.UserId == userId);

            await GenericDelete<Interaction>(x => x.UserId == userId);
            await GenericDelete<InteractionStatusLog>(x => x.Interaction.UserId == userId);
            await GenericDelete<InteractionAttachment>(x => x.Interaction.UserId == userId);

            await GenericDelete<ApplicationUserRole>(x => x.UserId == userId);
            await GenericDelete<ApplicationUser>(x => x.Id == userId);

            
            var res = await TrySaveChangesAndReturnResultAsync($"Пользователь {userToRemove.Email} удален");

            if(!res.IsSucceeded)
            {
                return res;
            }

            await Application.GetService<IProductService>(AmbientContext).RecalculateErrorProducts();

            return res;
        }

        public async Task GenericDelete<TEntity>(Expression<Func<TEntity, bool>> whereExpression) where TEntity : class
        {
            DeleteHandled(await Query<TEntity>().Where(whereExpression).ToListAsync());
        }

        /// <summary>
        /// Редактирование пользователя администратором
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponse> EditUserAsync(EditApplicationUser model)
        {
            var validation = ValidateModelAndUserIsAdmin(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            var clientRepo = GetRepository<Client>();

            var searcher = new UserSearcher(AmbientContext);

            var userDto = await searcher.GetUserByIdAsync(model.Id);
            
            if (userDto == null)
            {
                return new BaseApiResponse(false, ValidationMessages.UserIsNotFoundByIdentifier);
            }

            if (userDto.Email == RightsSettings.RootEmail)
            {
                return new BaseApiResponse(false, ValidationMessages.YouCantEditRootUser);
            }

            if(await clientRepo.Query().AnyAsync(x => x.Email == model.Email && x.Id != model.Id))
            {
                return new BaseApiResponse(false, ValidationMessages.ThisEmailIsAlreadyTaken);
            }
            
            if(await clientRepo.Query().AnyAsync(x => x.PhoneNumber == model.PhoneNumber && x.Id != model.Id))
            {
                return new BaseApiResponse(false, ValidationMessages.ThisPhoneNumberIsAlreadyTaken);
            }
            

            if(!User.HasRight(UserRight.Root) && (userDto.HasRight(UserRight.Admin) || userDto.HasRight(UserRight.SuperAdmin)))
            {
                return new BaseApiResponse(false, ValidationMessages.YouCantEditUserBecauseHeIsAdministrator);
            }

            if(!User.HasRight(UserRight.Root) && User.HasRight(UserRight.SuperAdmin) && userDto.HasRight(UserRight.SuperAdmin))
            {
                return new BaseApiResponse(false, ValidationMessages.YouCantEditUserBecauseHeIsSuperAdministrator);
            }

            if (!User.HasRight(UserRight.Root) && !User.HasRight(UserRight.SuperAdmin) && User.HasRight(UserRight.Admin) && userDto.HasRight(UserRight.Admin))
            {
                return new BaseApiResponse(false, "Вы не имеете прав Супер-Администратора, следовательно не можете редактировать пользователя, так как он является Администратором");
            }

            var userToEditEntity = await clientRepo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (userToEditEntity == null)
            {
                var ex = new Exception("Ужасная ошибка");

                Logger.LogException(ex);

                return new BaseApiResponse(ex);
            }

            
            userToEditEntity.Email = model.Email;
            userToEditEntity.Name = model.Name;
            userToEditEntity.Surname = model.Surname;
            userToEditEntity.Patronymic = model.Patronymic;
            userToEditEntity.Sex = model.Sex;
            userToEditEntity.ObjectJson = model.ObjectJson;
            userToEditEntity.PhoneNumber = new string(model.PhoneNumber.Where(char.IsDigit).ToArray());
            userToEditEntity.BirthDate = model.BirthDate;

            clientRepo.UpdateHandled(userToEditEntity);

            return await TrySaveChangesAndReturnResultAsync("Данные пользователя обновлены");
        }

        
        public async Task<BaseApiResponse> ActivateOrDeActivateUserAsync(UserActivation model)
        {
            var searcher = new UserSearcher(AmbientContext);

            var userDto = await searcher.GetUserByIdAsync(model.Id);

            if (userDto == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }
            
            var result = UserRightsWorker.HasRightToEditUser(userDto, User);
            
            if(!result.IsSucceeded)
            {
                return result;
            }

            var userRepo = GetRepository<Client>();

            var user = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по указанному идентификатору");
            }

            if (model.DeActivated)
            {
                if(user.DeActivated)
                {
                    return new BaseApiResponse(false, "Пользователь уже является деактивированным");
                }

                
                user.DeActivated = true;

                userRepo.UpdateHandled(user);

                return await TrySaveChangesAndReturnResultAsync("Пользователь деактивирован");
            }

            if(!user.DeActivated)
            {
                return new BaseApiResponse(false, "Пользователь уже активирован");
            }

            
            user.DeActivated = false;

            userRepo.UpdateHandled(user);

            return await TrySaveChangesAndReturnResultAsync("Пользователь активирован");
        }

        public UserWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}