using Croco.Core.Abstractions;
using System.Collections.Generic;

namespace Ecc.Contract.Models.Emails
{
    public class GetEmailSenderOptions
    {
        public ICrocoAmbientContext AmbientContext { get; set; }
        public List<EccFileData> Attachments { get; set; }
    }
}