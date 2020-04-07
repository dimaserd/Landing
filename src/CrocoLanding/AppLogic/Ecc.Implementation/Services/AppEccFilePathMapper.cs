using Croco.Core.Abstractions.Application;
using Ecc.Logic.Abstractions;

namespace Ecc.Implementation.Services
{
    public class AppEccFilePathMapper : IEccFilePathMapper
    {
        ICrocoApplication App { get; }

        public AppEccFilePathMapper(ICrocoApplication app)
        {
            App = app;
        }

        
        public string MapPath(string filePath)
        {
            return App.MapPath($"/wwwroot{filePath}");
        }
    }
}