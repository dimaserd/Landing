using Croco.Core.Data.Implementations.DbAudit.Models;
using Croco.Core.EventSourcing.Implementations.StatusLog.Models;
using Croco.Core.Model.Entities;
using Croco.Core.Model.Entities.Store;
using Microsoft.EntityFrameworkCore;

namespace Zoo.Core.Abstractions
{
    public interface IStoreContext
    {
        DbSet<AuditLog> AuditLogs { get; set; }

        DbSet<IntegrationMessageLog> IntegrationMessageLogs { get; set; }

        DbSet<IntegrationMessageStatusLog> IntegrationMessageStatusLogs { get; set; }

        DbSet<LoggedApplicationAction> LoggedApplicationActions { get; set; }

        DbSet<LoggedUserInterfaceAction> LoggedUserInterfaceActions { get; set; }

        DbSet<RobotTask> RobotTasks { get; set; }

        DbSet<WebAppRequestContextLog> WebAppRequestContextLogs { get; set; }
    }
}