using Croco.Core.Abstractions.Settings;

namespace Clt.Contract.Settings
{
    public class CltRolesSetting : ICommonSetting<CltRolesSetting>
    {
        public string AdminRoleName { get; set; }

        public string RootRoleName { get; set; }

        public CltRolesSetting GetDefault()
        {
            return new CltRolesSetting
            {
                AdminRoleName = "Admin",
                RootRoleName = "Root"
            };
        }
    }
}