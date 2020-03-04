using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrocoLanding.Model.Entities.Clt.Default
{
    [Table(nameof(ApplicationUserRole), Schema = Schemas.CltSchema)]
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}