using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cmn.Enums;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Extensions.Enumerations;
using CrocoShop.Logic.Resources;
using CrocoShop.Logic.Workers.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CrocoShop.Model.Entities.Clt.Default;
using Clt.Contract.Models.Users;

namespace Clt.Logic.Workers.Users
{
    public class UserRoleWorker : BaseWorker
    {
        public static int GetTheHighestRoleOfUser(IList<string> roles)
        {
            var rights = new[]
            {
                UserRight.Root, UserRight.SuperAdmin, UserRight.Admin
            };

            foreach(var right in rights)
            {
                if(roles.Contains(right.ToString()))
                {
                    return (int)right;
                }
            }

            return int.MaxValue;
        }

        public async Task<BaseApiResponse> AddUserToRoleAsync(UserIdAndRole userIdAndRole, UserManager<ApplicationUser> userManager)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, "Вы не авторизованы");
            }
            var role = await Query<ApplicationRole>().FirstOrDefaultAsync(x => x.Name == userIdAndRole.Role.ToString());

            if (role == null)
            {
                return new BaseApiResponse(false, "Право не найдено (Возможно оно еще не создано)");
            }

            var userRepo = GetRepository<ApplicationUser>();

            var user = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == userIdAndRole.UserId);
            if (user == null)
            {
                return new BaseApiResponse(false, "Изменяемый пользователь не найден");
            }

            var userEditorId = UserId;
            var userEditor = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == userEditorId);
            if (userEditor == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }
            
            var rolesOfEditUser = await userManager.GetRolesAsync(userEditor);
            var rolesOfUserEditor = await userManager.GetRolesAsync(userEditor);

            var roleOfEditUser = GetTheHighestRoleOfUser(rolesOfEditUser);
            var roleOfUserEditor = GetTheHighestRoleOfUser(rolesOfUserEditor);

            if (roleOfUserEditor >= roleOfEditUser)
            {
                return new BaseApiResponse(false, "Вы не имеете прав изменять роль данного пользователя");
            }

            if (roleOfUserEditor >= (int)userIdAndRole.Role)
            {
                return new BaseApiResponse(false, "Вы не имеете прав добавлять роль равную или выше вашей");
            }

            if (user.Roles.Any(x => x.RoleId == role.Id))
            {
                return new BaseApiResponse(false, "У пользователя уже есть данное право");
            }

            var addingToRoleResult = await userManager.AddToRoleAsync(userEditor, role.Name);

            if (!addingToRoleResult.Succeeded)
            {
                return new BaseApiResponse(false, addingToRoleResult.Errors.First().Description);
            }

            return new BaseApiResponse(true, $"Право {userIdAndRole.Role.ToDisplayName()} добавлено пользователю {user.Id}"); ;
        }

        
        public async Task<BaseApiResponse> RemoveRoleFromUserAsync(UserIdAndRole userIdAndRole, UserManager<ApplicationUser> userManager)
        {
            if (!IsAuthenticated)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAuthorized);
            }

            var role = await Query<ApplicationRole>().FirstOrDefaultAsync(x => x.Name == userIdAndRole.Role.ToString());

            if (role == null)
            {
                return new BaseApiResponse(false, "Право не найдено (Возможно оно еще не создано)");
            }

            var userRepo = GetRepository<ApplicationUser>();

            var user = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == userIdAndRole.UserId);
            if (user == null)
            {
                return new BaseApiResponse(false, "Изменяемый пользователь не найден");
            }

            var userEditorId = UserId;
            var userEditor = await userRepo.Query().FirstOrDefaultAsync(x => x.Id == userEditorId);
            if (userEditor == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден");
            }

            var rolesOfEditUser = await userManager.GetRolesAsync(user);
            var rolesOfUserEditor = await userManager.GetRolesAsync(userEditor);

            var roleOfEditUser = GetTheHighestRoleOfUser(rolesOfEditUser);
            var roleOfUserEditor = GetTheHighestRoleOfUser(rolesOfUserEditor);

            if (roleOfUserEditor >= roleOfEditUser)
            {
                return new BaseApiResponse(false, "Вы не имеете прав изменять роль данного пользователя");
            }

            if (roleOfUserEditor >= (int)userIdAndRole.Role)
            {
                return new BaseApiResponse(false, "Вы не имеете прав удалять роль равную или выше вашей");
            }

            if (user.Roles.All(x => x.RoleId != role.Id))
            {
                return new BaseApiResponse(false, "У пользователя уже отсутствует данное право");
            }

            var removingToRoleResult = await userManager.RemoveFromRoleAsync(userEditor, role.Name);

            if (!removingToRoleResult.Succeeded)
            {
                return new BaseApiResponse(false, removingToRoleResult.Errors.First().Description);
            }

            return new BaseApiResponse(true, $"Право {userIdAndRole.Role.ToDisplayName()} удалено у пользователя {user.Id}"); ;
        }

        public UserRoleWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}