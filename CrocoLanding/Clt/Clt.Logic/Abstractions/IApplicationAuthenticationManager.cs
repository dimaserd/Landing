using System.Threading.Tasks;

namespace Clt.Logic.Abstractions
{
    public interface IApplicationAuthenticationManager
    {
        void SignOut();

        Task SignOutAsync();
    }
}