using Clt.Contract.Models.Common;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Clt.Logic.Core.Workers
{
    public class CoreCltAccountLoginWorker<TUser> : BaseCltWorker where TUser : IdentityUser
    {
        public CoreCltAccountLoginWorker(ICrocoAmbientContext context) : base(context)
        {
        }

        public async Task<BaseApiResponse> LoginAsUserAsync(UserIdModel model, SignInManager<TUser> signInManager)
        {
            var validation = ValidateModel(model);

            if (!validation.IsSucceeded)
            {
                return validation;
            }

            if (!IsUserRoot())
            {
                return new BaseApiResponse(false, "У вас недостаточно прав для логинирования за другого пользователя");
            }

            var user = await signInManager.UserManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return new BaseApiResponse(false, "Пользователь не найден по укаанному идентификатору");
            }

            await signInManager.SignInAsync(user, true);

            return new BaseApiResponse(true, $"Вы залогинены как {user.Email}");
        }
    }
}