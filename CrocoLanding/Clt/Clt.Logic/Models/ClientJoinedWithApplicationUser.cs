using CrocoLanding.Model.Entities.Clt;
using CrocoLanding.Model.Entities.Clt.Default;

namespace Clt.Logic.Models
{
    public class ClientJoinedWithApplicationUser
    {
        public Client Client { get; set; }

        public ApplicationUser User { get; set; }
    }
}