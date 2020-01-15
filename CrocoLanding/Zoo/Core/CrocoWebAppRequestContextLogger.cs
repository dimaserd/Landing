using Croco.Core.Abstractions;
using Croco.Core.Abstractions.Data;
using Croco.Core.Application;
using System;

namespace Zoo.Core
{
    public class CrocoWebAppRequestContextLogger : ICrocoRequestContextLogger
    {
        public void LogRequestContext(ICrocoAmbientContext ambientContext)
        {
            var log = GetLog(ambientContext.RequestContext);

            ambientContext.RepositoryFactory
                .GetRepository<WebAppRequestContextLog>().CreateHandled(log);

            ambientContext.RepositoryFactory.SaveChangesAsync<WebAppRequestContextLog>().GetAwaiter().GetResult();
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