using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Data;
using Croco.Core.Application;
using Croco.Core.Implementations.TransactionHandlers;
using System;

namespace Zoo.Core
{
    public class CrocoWebAppRequestContextLogger : ICrocoRequestContextLogger
    {
        public void LogRequestContext(ICrocoAmbientContext ambientContext)
        {
            
        }

        public void LogRequestContext(ICrocoRequestContext requestContext)
        {
            CrocoTransactionHandler.System.ExecuteAndCloseTransaction(amb =>
            {
                var log = GetLog(requestContext);

                amb.RepositoryFactory
                    .GetRepository<WebAppRequestContextLog>().CreateHandled(log);

                return amb.RepositoryFactory.SaveChangesAsync<WebAppRequestContextLog>();
            }).GetAwaiter().GetResult();
        }

        private WebAppRequestContextLog GetLog(ICrocoRequestContext requestContext)
        {
            var now = CrocoApp.Application.DateTimeProvider.Now;

            var log = new WebAppRequestContextLog
            {
                Id = requestContext.RequestId,
                ParentRequestId = requestContext.ParentRequestId,
                RequestId = requestContext.RequestId,
                UserId = requestContext.UserPrincipal.UserId,
                Uri = null,
                FinishedOn = now,
                StartedOn = DateTime.MinValue
            };

            if (requestContext is MyWebAppCrocoRequestContext webRequestContext)
            {
                log.Uri = webRequestContext.Uri;
                log.StartedOn = webRequestContext.StartedOn;
            }

            return log;
        }
    }
}