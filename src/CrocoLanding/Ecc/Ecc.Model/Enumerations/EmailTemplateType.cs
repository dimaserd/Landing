using System.ComponentModel.DataAnnotations;

namespace Ecc.Model.Enumerations
{
    public enum EmailTemplateType
    {
        [Display(Name = "Не указано")]
        NotSet,

        [Display(Name = "Регистрация")]
        Registration,

        [Display(Name = "Заказ выполнен")]
        PurchaseCompleted,

        [Display(Name = "Забыл пароль")]
        UserForgotPassword
    }
}