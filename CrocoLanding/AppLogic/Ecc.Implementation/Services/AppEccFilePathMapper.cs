using Croco.Core.Application;
using Ecc.Logic.Abstractions;

namespace Ecc.Implementation.Services
{
    public class AppEccFilePathMapper : IEccFilePathMapper
    {
        public string MapPath(string filePath)
        {
            return CrocoApp.Application.MapPath($"/wwwroot{filePath}");
        }
    }
}