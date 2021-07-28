using Clt.Model;
using Croco.Common.Options;
using Croco.Common.Registrators;
using Croco.Core.Logic.DbContexts;
using CrocoLanding.Model.Contexts;
using Ecc.Model.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrocoLanding.Registrators
{
    public static class AppDbContextRegistrator
    {
        public static void RegisterDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = configuration.GetSection(nameof(DbConnectionOptions)).Get<DbConnectionOptions>();

            DbContextRegistratorExtensions.RegisterDbContexts(services, registrator =>
            {
                registrator.RegisterDbContext<LandingDbContext>(dbOptions.Connections["Landing"]);
                registrator.RegisterDbContext<EccDbContext>(dbOptions.Connections["Ecc"]);
                registrator.RegisterDbContext<CltDbContext>(dbOptions.Connections["Clt"]);
                registrator.RegisterDbContext<CrocoInternalDbContext>(dbOptions.Connections["Croco"]);
            });
        }
    }
}
