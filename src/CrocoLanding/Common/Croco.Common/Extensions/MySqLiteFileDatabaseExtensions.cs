 using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Croco.Common.Extensions
{
    public class MySqLiteFileDatabaseExtensions
    {
        public static TContext Create<TContext>(Func<DbContextOptions<TContext>, TContext> createFunc, string dbName) where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder = ConfigureBuilder(optionsBuilder, dbName);

            return createFunc(optionsBuilder.Options);
        }

        public static DbContextOptionsBuilder<TContext> ConfigureBuilder<TContext>(DbContextOptionsBuilder<TContext> builder, string dbName) where TContext : DbContext
        {
            builder.UseSqlite($"Data Source={dbName}.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            return builder;
        }

        public static DbContextOptionsBuilder ConfigureBuilder(DbContextOptionsBuilder builder, string dbName)
        {
            builder.UseSqlite($"Data Source={dbName}.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            return builder;
        }
    }
}