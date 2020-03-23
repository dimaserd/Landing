using Croco.Core.Abstractions.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CrocoLanding.CrocoStuff
{
    public class StartUpCrocoOptions
    {
        public string ApplicationUrl { get; set; }

        public IConfiguration Configuration { get; set; }

        public IWebHostEnvironment Env { get; set; }

        public List<Action<ICrocoApplication>> ApplicationActions { get; set; } = new List<Action<ICrocoApplication>>();

        public List<Action<ICrocoApplicationOptions>> BuildActions { get; set; } = new List<Action<ICrocoApplicationOptions>>();

        public List<Action<IServiceCollection>> ServiceRegistrations { get; set; } = new List<Action<IServiceCollection>>();
    }
}