using Croco.Core.Abstractions;

namespace Ecc.Implementation.Services
{
    public interface IEccTextFunctionInvoker
    {
        string ProccessText(string interactionId, EccReplacing replacing, ICrocoAmbientContext ambientContext);
    }
}