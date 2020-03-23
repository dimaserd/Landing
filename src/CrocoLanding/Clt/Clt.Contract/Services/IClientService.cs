using Clt.Contract.Models.Account;
using Clt.Contract.Models.Users;
using Croco.Core.Abstractions.Models;
using System.Threading.Tasks;

namespace Clt.Contract.Services
{
    public interface IClientService
    {
        Task<BaseApiResponse<RegisteredUser>> RegisterAndSignInAsync(RegisterModel model, bool createRandomPassword, object userManager, object signInManager);
    }
}