using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Abstractions.Data.Entities.HaveId;
using Croco.Core.Model.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace CrocoLanding.Model.Entities.Clt.Default
{
    [Table(nameof(ApplicationUser), Schema = Schemas.CltSchema)]
    public class ApplicationUser : IdentityUser, IAuditableEntity, IHaveStringId
    {
        public ICollection<ApplicationUserRole> Roles { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string LastModifiedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}