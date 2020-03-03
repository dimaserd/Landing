using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Logic.Workers;
using Ecc.Contract.Settings;
using Ecc.Logic.Resources;

namespace Ecc.Logic.Workers.Base
{
    public class BaseEccWorker : BaseCrocoWorker
    {
        EccRolesSetting RolesSetting { get; }

        public BaseEccWorker(ICrocoAmbientContext context) : base(context)
        {
            RolesSetting = Application.SettingsFactory.GetSetting<EccRolesSetting>();
        }

        public bool IsUserAdmin()
        {
            return User.IsInRole(RolesSetting.AdminRoleName);
        }

        public BaseApiResponse ValidateModelAndUserIsAdmin(object model)
        {
            var right = IsUserAdmin();

            if (!right)
            {
                return new BaseApiResponse(false, ValidationMessages.YouAreNotAnAdministrator);
            }

            return ValidateModel(model);
        }
    }
}