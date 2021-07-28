using Croco.Core.Application;
using Croco.Core.Application.Registrators;
using Croco.Core.Logic.DbContexts;
using Croco.Core.Logic.Files;
using CrocoLanding.Logic.Services;
using CrocoLanding.Model.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace CrocoLanding.Logic
{
    public static class CrocoLandingLogicRegistrator
    {
        public static void RegisterServices(CrocoApplicationBuilder applicationBuilder)
        {
            DbFileManagerServiceCollectionExtensions.RegisterDbFileManager<CrocoInternalDbContext>(applicationBuilder);
            Check(applicationBuilder);
            applicationBuilder.Services.AddScoped<CallBackRequestWorker>();
        }

        private static void Check(CrocoApplicationBuilder applicationBuilder)
        {
            new EFCrocoApplicationRegistrator(applicationBuilder).RegiterIfNeedEFDataCoonection<LandingDbContext>();
        }
    }
}