using Croco.Core.Abstractions.Application;
using Croco.Core.Implementations.AmbientContext;
using Croco.Core.Implementations.TransactionHandlers;
using System.Threading.Tasks;

namespace CrocoLanding.CrocoStuff
{
    public class ApplicationServiceRegistrator
    {
        public static void Register(ICrocoApplication application)
        {
            LogApplicationInit();
        }

        public static void LogApplicationInit()
        {
            new CrocoTransactionHandler(() => new SystemCrocoAmbientContext()).ExecuteAndCloseTransactionSafe(amb =>
            {
                amb.Logger.LogInfo("App.Initialized", "Приложение инициализировано");

                return Task.CompletedTask;
            }).GetAwaiter().GetResult();
        }
    }
}