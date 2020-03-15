using Clt.Contract.Settings;
using Clt.Logic.Core.Resources;
using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Logic.Workers;

namespace Clt.Logic.Core.Workers
{
    public class BaseCltWorker : BaseCrocoWorker
    {
        protected CltRolesSetting RolesSetting { get; }

        public BaseCltWorker(ICrocoAmbientContext context) : base(context)
        {
            RolesSetting = Application.SettingsFactory.GetSetting<CltRolesSetting>();
        }

        public bool IsUserAdmin()
        {
            return IsUserRoot() || User.IsInRole(RolesSetting.AdminRoleName);
        }

        public bool IsUserRoot()
        {
            return User.IsInRole(RolesSetting.RootRoleName);
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