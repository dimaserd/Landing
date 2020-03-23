using Croco.Core.Abstractions.DependencyInjection;
using System;

namespace CrocoLanding.CrocoStuff
{
    public class CrocoActivator : ICrocoActivator
    {
        public CrocoActivator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public object ActivateInstance(Type type)
        {
            return ServiceProvider.GetService(type);
        }
    }
}