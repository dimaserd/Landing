using Croco.Common.Options;

namespace Croco.Common
{
    public class StartUpCrocoOptions
    {
        public AppOptions AppOptions { get; set; }
        public string ContentRootPath { get; set; }
        public string WebRootPath { get; set; }
    }
}