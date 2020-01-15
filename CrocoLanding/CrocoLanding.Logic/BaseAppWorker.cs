using Croco.Core.Abstractions;
using Croco.Core.Logic.Workers;

namespace CrocoLanding.Logic
{
    public class BaseAppWorker : BaseCrocoWorker<LandingWebApplication>
    {
        public BaseAppWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }
    }
}