using Croco.Core.EventSourcing.Implementations;
using Croco.Core.Extensions;
using System.Threading.Tasks;

namespace Zoo.Core
{
    public class CrocoWebAppRequestContextLogHandler : CrocoMessageHandler<WebAppRequestContextLog>
    {
        public override Task HandleMessage(WebAppRequestContextLog model)
        {
            return GetSystemTransactionHandler().ExecuteAndCloseTransactionSafe(amb =>
            {
                amb.RepositoryFactory.CreateHandled(model);

                return amb.RepositoryFactory.SaveChangesAsync();
            });
        }
    }
}