using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Models;
using Croco.Core.Data.Implementations.DbAudit.Models;
using Croco.Core.EventSourcing.Implementations.StatusLog.Models;
using Croco.Core.Model.Entities.Store;
using CrocoLanding.Logic.Extensions;
using System.Threading.Tasks;

namespace CrocoLanding.Logic.Workers
{
    public class DeleteDataWorker : BaseAppWorker
    {
        public DeleteDataWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public async Task<BaseApiResponse> DeleteLogs()
        {
            if (!User.IsAdmin())
            {
                return new BaseApiResponse(false, "У вас недостаточно прав");
            }

            await Query<LoggedApplicationAction>().DeleteFromQueryAsync();
            await Query<AuditLog>().DeleteFromQueryAsync();
            await Query<IntegrationMessageStatusLog>().DeleteFromQueryAsync();
            await Query<IntegrationMessageLog>().DeleteFromQueryAsync();

            return new BaseApiResponse(true, "Очищены данные");
        }
    }
}