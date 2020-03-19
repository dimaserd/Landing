using System;
using System.Threading.Tasks;
using Croco.Core.Implementations.TransactionHandlers;
using CrocoLanding.Abstractions;

namespace CrocoLanding.Implementations
{
    public class ApplicationLoggerManager : ILoggerManager
    {
        public Task LogExceptionAsync(Exception ex)
        {
            if (ex == null)
            {
                return Task.CompletedTask;
            }

            return CrocoTransactionHandler.System.ExecuteAndCloseTransaction(ctx =>
            {
                ctx.Logger.LogException("ApplicationLoggerManager.LogExceptionAsync", ex);

                return Task.CompletedTask;
            });
        }
    }
}