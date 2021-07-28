using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Croco.Common.Services
{
    public class DbCreator
    {
        private static readonly List<Type> _dbContextTypes = new();

        private readonly List<DbContext> _dbContexts = new();

        public DbCreator(IServiceProvider serviceProvider)
        {
            foreach (var dbContextType in _dbContextTypes)
            {
                _dbContexts.Add(serviceProvider.GetRequiredService(dbContextType) as DbContext);
            }
        }

        public static void AddDbContext<TDbContext>() where TDbContext : DbContext
        {
            _dbContextTypes.Add(typeof(TDbContext));
        }

        public void CreateDatabases()
        {
            foreach (var db in _dbContexts)
            {
                db.Database.EnsureCreated();
            }
        }
    }
}