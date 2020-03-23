using System.Collections.Generic;

namespace Ecc.Model.Entities.LinkCatch
{
    public class EmailLinkCatch
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public virtual ICollection<EmailLinkCatchRedirect> Redirects { get; set; }
    }
}