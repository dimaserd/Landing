using System.ComponentModel.DataAnnotations;

namespace Ecc.Model.Enumerations
{
    public enum IntegratedAppType
    {
        [Display(Name = "iOS Application")]
        IosApplication,

        [Display(Name = "Android Application")]
        AndroidApplication
    }
}