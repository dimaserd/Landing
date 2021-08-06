using Clt.Model;
using Croco.Core.Logic.DbContexts;
using Dlv.Model.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prd.Model.Contexts;
using System.Collections.Generic;
using System;
using Croco.Common.Extensions;
using Croco.Common.Options;
using Croco.Common.Registrators;
using CrocoShop.Web.Model.Contexts;

namespace CrocoShop.Web.Registrators
{
    public static class AppDbContextRegistrator
    {
        public static void RegisterDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = configuration.GetSection(nameof(DbConnectionOptions)).Get<DbConnectionOptions>();

            DbContextRegistratorExtensions.RegisterDbContexts(services, registrator =>
            {
                registrator.RegisterDbContext<CourtRecordDbContext>(dbOptions.Connections["Court"]);
                registrator.RegisterDbContext<CltDbContext>(dbOptions.Connections["Clt"]);
                registrator.RegisterDbContext<CrocoInternalDbContext>(dbOptions.Connections["Croco"]);
                registrator.RegisterDbContext<PrdDbContext>(dbOptions.Connections["Prd"]);
                registrator.RegisterDbContext<DlvDbContext>(dbOptions.Connections["Dlv"]);
            });
        }
    }
}