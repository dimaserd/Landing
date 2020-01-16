using CrocoShop.Model.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace CrocoLanding.Model
{
    public class LandingDbContextFactory : IDesignTimeDbContextFactory<LandingDbContext>
    {
        private readonly string ServerConnectionString = "Server=wpl32.hosting.reg.ru;Database=u0803867_CrocoShop;Persist Security Info=True;Pooling=false;User ID=u0803867_CrocoShop;Password=&Sf05or1";
        public static readonly string LocalConnectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-CrocoLanding-12bc9b9d-9d6a-45d4-4829-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true";

        public LandingDbContext CreateDbContext(string[] args)
        {
            return LandingDbContext.Create(LocalConnectionString);
        }

        public void ExecuteCommand(string commandText)
        {
            using var db = CreateDbContext(null);
            using var command = db.Database.GetDbConnection().CreateCommand();
            command.CommandText = commandText;
            db.Database.OpenConnection();
            using var result = command.ExecuteReader();
            // do something with result
            Console.WriteLine(result.ToString());
        }
    }
}