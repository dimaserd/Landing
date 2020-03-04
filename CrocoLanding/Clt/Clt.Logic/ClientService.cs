using Clt.Contract.Models.Account;
using Clt.Contract.Models.Users;
using Clt.Contract.Services;
using Clt.Logic.Workers.Account;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using CrocoShop.Model.Entities.Clt.Default;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Clt.Logic
{
    public class ClientService : IClientService
    {
        public ClientService(ICrocoAmbientContext ambientContext)
        {
            AmbientContext = ambientContext;
        }

        ICrocoAmbientContext AmbientContext { get; }

        public Task<BaseApiResponse<RegisteredUser>> RegisterAndSignInAsync(RegisterModel model, bool createRandomPassword, object userManager, object signInManager)
        {
            return new AccountRegistrationWorker(AmbientContext)
                .RegisterAndSignInAsync(model, createRandomPassword, userManager as UserManager<ApplicationUser>, signInManager as SignInManager<ApplicationUser>);
        }
    }
}