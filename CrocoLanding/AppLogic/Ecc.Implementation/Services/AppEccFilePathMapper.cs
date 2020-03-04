using Croco.Core.Application;
using Croco.Core.Extensions;
using CrocoLanding.Logic;
using Ecc.Logic.Abstractions;

namespace Ecc.Implementation.Services
{
    public class AppEccFilePathMapper : IEccFilePathMapper
    {
        public string MapPath(string filePath)
        {
            return CrocoApp.Application.As<LandingWebApplication>().MapPath(filePath);
        }
    }
}