using Croco.WebApplication.Application;
using Microsoft.AspNetCore.StaticFiles;

namespace CrocoLanding.Logic
{
    public class LandingWebApplication : CrocoWebApplication
    {
        public LandingWebApplication(CrocoWebApplicationOptions options) : base(options)
        {
        }

        public static string GetMimeMapping(string fileName)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);

            return contentType ?? "application/octet-stream";
        }

        public static bool IsImage(string fileName)
        {
            return GetMimeMapping(fileName).StartsWith("image/");
        }
    }
}