using Croco.Core.Contract.Data.Entities.HaveId;
using Croco.Core.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace CrocoLanding.Model.Entities.Ecc
{
    public class CallBackRequest : AuditableEntityBase, IHaveStringId
    {
        public string Id { get; set; }

        [MaxLength(64)]
        public string EmailOrPhoneNumber { get; set; }

        [MaxLength(64)]
        public string IpAddress { get; set; }

        public bool IsNotified { get; set; }
    }
}