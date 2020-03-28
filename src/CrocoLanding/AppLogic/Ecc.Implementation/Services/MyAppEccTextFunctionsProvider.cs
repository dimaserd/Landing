using Ecc.Logic.Abstractions;
using Ecc.Logic.Services;
using System.Collections.Generic;

namespace Ecc.Implementation.Services
{
    public class MyAppEccTextFunctionsProvider : IEccTextFunctionsProvider
    {
        string UrlRedirectFormat { get; }

        public MyAppEccTextFunctionsProvider(string urlRedirectFormat)
        {
            UrlRedirectFormat = urlRedirectFormat;
        }
        
        public Dictionary<string, IEccTextFunctionInvoker> GetFunctions()
        {
            return new Dictionary<string, IEccTextFunctionInvoker>
            {
                ["LinkTo"] = new EccLinkFunctionInvoker(UrlRedirectFormat)
            };
        }
    }
}