using Ecc.Model.Entities.LinkCatch;

namespace Ecc.Logic.Abstractions
{
    public interface IEccEmailLinkSubstitutor
    {
        (string, EmailLinkCatch[]) ProcessEmailText(string body, string mailMessageId); 
    }
}