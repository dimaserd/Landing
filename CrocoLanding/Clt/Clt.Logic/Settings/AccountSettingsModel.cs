using Croco.Core.Abstractions.Settings;
using System.ComponentModel.DataAnnotations;

namespace Clt.Logic.Settings
{
    public class AccountSettingsModel : ICommonSetting<AccountSettingsModel>
    {
        [Display(Name = "Разрешить авторизацию тем, кто не подтвердил почту")]
        public bool IsLoginEnabledForUsersWhoDidNotConfirmEmail { get; set; }

        [Display(Name = "Должны ли пользователи подтвержать почту")]
        public bool ShouldUsersConfirmEmail { get; set; }

        [Display(Name = "Разрешена ли регистрация")]
        public bool RegistrationEnabled { get; set; }

        [Display(Name = "Тип подтверждения логина")]
        public ConfirmLoginType ConfirmLogin { get; set; }

        public AccountSettingsModel GetDefault()
        {
            return new AccountSettingsModel
            {
                IsLoginEnabledForUsersWhoDidNotConfirmEmail = true,
                ShouldUsersConfirmEmail = false,
                RegistrationEnabled = true,
                ConfirmLogin = ConfirmLoginType.None
            };
        }

        public enum ConfirmLoginType
        {
            [Display(Name = "Не указано")]
            None,
        }
    }
}