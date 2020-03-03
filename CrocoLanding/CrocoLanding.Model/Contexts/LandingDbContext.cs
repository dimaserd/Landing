using Croco.Core.Data.Implementations.DbAudit.Models;
using Croco.Core.EventSourcing.Implementations.StatusLog.Models;
using Croco.Core.Model.Entities;
using Croco.Core.Model.Entities.Store;
using CrocoLanding.Model.Abstractions;
using CrocoLanding.Model.Entities.Ecc;
using Ecc.Model.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Zoo.Core;

namespace CrocoShop.Model.Contexts
{
    public partial class LandingDbContext : DbContext, IStoreContext
    {
        public const string ServerConnection = "ServerConnection";

        public const string LocalConnection = "DefaultConnection";

#if DEBUG
        public static string ConnectionString => LocalConnection;
#else
        public static string ConnectionString => ServerConnection;
#endif

        #region Конструкторы
        public LandingDbContext(DbContextOptions<LandingDbContext> options) : base(options)
        {
        }
        #endregion

        public DbSet<CallBackRequest> CallBackRequests { get; set; }

        #region Store

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<IntegrationMessageLog> IntegrationMessageLogs { get; set; }

        public DbSet<IntegrationMessageStatusLog> IntegrationMessageStatusLogs { get; set; }

        public DbSet<LoggedApplicationAction> LoggedApplicationActions { get; set; }

        public DbSet<LoggedUserInterfaceAction> LoggedUserInterfaceActions { get; set; }

        public DbSet<RobotTask> RobotTasks { get; set; }

        public DbSet<WebAppRequestContextLog> WebAppRequestContextLogs { get; set; }

        #endregion

        public static LandingDbContext Create(IConfiguration configuration)
        {
            return Create(configuration.GetConnectionString(ConnectionString));
        }

        public static LandingDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LandingDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LandingDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            LoggedApplicationAction.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditLog>().Property(x => x.Id).ValueGeneratedOnAdd();

            WebAppRequestContextLog.OnModelCreating(modelBuilder);

            EccDbContext.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}