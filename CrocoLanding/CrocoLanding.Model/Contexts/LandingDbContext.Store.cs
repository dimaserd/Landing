using Croco.Core.Data.Implementations.DbAudit.Models;
using Croco.Core.EventSourcing.Implementations.StatusLog.Models;
using Croco.Core.Model.Entities;
using Croco.Core.Model.Entities.Store;
using Microsoft.EntityFrameworkCore;
using Zoo.Core;

namespace CrocoLanding.Model.Contexts
{
    public partial class LandingDbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<IntegrationMessageLog> IntegrationMessageLogs { get; set; }

        public DbSet<IntegrationMessageStatusLog> IntegrationMessageStatusLogs { get; set; }

        public DbSet<LoggedApplicationAction> LoggedApplicationActions { get; set; }

        public DbSet<LoggedUserInterfaceAction> LoggedUserInterfaceActions { get; set; }

        public DbSet<RobotTask> RobotTasks { get; set; }

        public DbSet<WebAppRequestContextLog> WebAppRequestContextLogs { get; set; }
    }
}