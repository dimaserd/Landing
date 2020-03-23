using Cmn.Enums;
using System.Security.Principal;

namespace CrocoLanding.Logic.Extensions
{
    public static class MyIdentityExtensions
    {
        public static bool IsAdmin(this IPrincipal rolePrincipal)
        {
            return rolePrincipal.HasRight(UserRight.Admin) || rolePrincipal.HasRight(UserRight.Root);
        }

        public static bool HasRight(this IPrincipal rolePrincipal, UserRight right)
        {
            return rolePrincipal.IsInRole(right.ToString());
        }
    }
}