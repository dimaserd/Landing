using Croco.Core.Abstractions.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CrocoShop.CrocoStuff
{
    public class StartUpCrocoOptions
    {
        public IConfiguration Configuration { get; set; }

        public IWebHostEnvironment Env { get; set; }

        public List<Action<ICrocoApplication>> ApplicationActions { get; set; } = new List<Action<ICrocoApplication>>();
    }
}