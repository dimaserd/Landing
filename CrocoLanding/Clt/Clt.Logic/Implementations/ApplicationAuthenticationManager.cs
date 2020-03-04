using System.Threading.Tasks;
using Clt.Logic.Abstractions;
using CrocoLanding.Model.Entities.Clt.Default;

namespace Clt.Logic.Implementations
{
    public class ApplicationAuthenticationManager : IApplicationAuthenticationManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationAuthenticationManager(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public void SignOut()
        {
            SignOutAsync().GetAwaiter().GetResult();
        }

        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }
    }
}