using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CrocoLanding.Model.Entities.Clt.Default
{
    [Table(nameof(ApplicationRole), Schema = Schemas.CltSchema)]
    public class ApplicationRole : IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}