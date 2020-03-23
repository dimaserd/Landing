using Croco.Core.Abstractions.Data.Entities.HaveId;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecc.Model.Entities.External
{
    [Table(nameof(EccUser), Schema = Schemas.EccSchema)]
    public class EccUser : IHaveStringId
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}