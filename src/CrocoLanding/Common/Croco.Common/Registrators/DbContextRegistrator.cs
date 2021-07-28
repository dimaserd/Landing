using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Croco.Common.Extensions;
using Croco.Common.Options;
using Croco.Common.Services;

namespace Croco.Common.Registrators
{
    public class DbContextRegistrator
    {
        IServiceCollection Services { get; }

        public DbContextRegistrator(IServiceCollection services)
        {
            Services = services;
        }

        public void RegisterDbContext<TDbContext>(DbConnectionOptions.DbConnection connection)
            where TDbContext : DbContext
        {
            if (connection.Type == DbConnectionOptions.DbType.SqLiteFile)
            {
                Services.AddDbContext<TDbContext>(opts => MySqLiteFileDatabaseExtensions.ConfigureBuilder(opts, connection.SqLiteFileDatabaseName));
            }
            else
            {
                Services.AddDbContext<TDbContext>(options =>
                    options.UseSqlServer(connection.ConnectionString));
            }

            DbCreator.AddDbContext<TDbContext>();
        }
    }

    public static class DbContextRegistratorExtensions
    {
        public static void RegisterDbContexts(this IServiceCollection services, Action<DbContextRegistrator> configureAction)
        {
            configureAction(new DbContextRegistrator(services));

            services.AddTransient<DbCreator>();
        }
    }
}