using Croco.Core.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CrocoLanding.CrocoStuff
{
    public class CrocoServiceRegistrator : ICrocoServiceRegistrator
    {
        public CrocoServiceRegistrator(IServiceCollection serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceCollection ServiceProvider { get; }

        public void AddTransient(Type type)
        {
            ServiceProvider.AddTransient(type);
        }
    }
}