using System.Threading.Tasks;

namespace Ecc.Logic.Abstractions
{
    public interface IEccEmailLinkSubstitutor
    {
        Task<string> ProcessEmailText(string body); 
    }
}