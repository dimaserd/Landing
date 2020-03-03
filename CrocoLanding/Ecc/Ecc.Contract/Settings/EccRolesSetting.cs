using Croco.Core.Abstractions.Settings;

namespace Ecc.Contract.Settings
{
    public class EccRolesSetting : ICommonSetting<EccRolesSetting>
    {
        public string AdminRoleName { get; set; }

        public EccRolesSetting GetDefault()
        {
            return new EccRolesSetting
            {
                AdminRoleName = "Admin",
            };
        }
    }
}