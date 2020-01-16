using Croco.WebApplication.Application;

namespace CrocoLanding.Logic
{
    public class LandingWebApplication : CrocoWebApplication
    {
        public LandingWebApplication(CrocoWebApplicationOptions options) : base(options)
        {
        }

        public bool IsDevelopment { get; set; }
    }
}