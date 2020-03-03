﻿using Croco.Core.Abstractions;
using Ecc.Logic.Models;
using Ecc.Logic.Models.Interactions;
using Ecc.Logic.Workers.Base;
using Ecc.Model.Entities.Interactions;
using Ecc.Model.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecc.Logic.Workers.Messaging
{
    public class ApplicationInteractionWorker : BaseEccWorker
    {
        public ApplicationInteractionWorker(ICrocoAmbientContext ambientContext) : base(ambientContext)
        {
        }

        public static IQueryable<ApplicationInteractionWithStatus<TInteraction>> GetQueryWithStatus<TInteraction>(IQueryable<TInteraction> query)
            where TInteraction : Interaction, new()
        {
            return query.Select(x => new ApplicationInteractionWithStatus<TInteraction>
            {
                Interaction = x,
                Status = x.Statuses.OrderByDescending(t => t.StartedOn).FirstOrDefault().Status
            });
        }

        public Task SetStatusForInteractions(IEnumerable<string> ids, InteractionStatus status, string statusDescription)
        {
            var now = Application.DateTimeProvider.Now;

            var list = ids.Select(x => new InteractionStatusLog
            {
                Id = Guid.NewGuid().ToString(),
                Status = status,
                StartedOn = now,
                InteractionId = x,
                StatusDescription = statusDescription
            });

            GetRepository<InteractionStatusLog>().CreateHandled(list);
            return SaveChangesAsync();
        }

        public Task UpdateInteractionStatusesAsync(List<UpdateInteractionStatus> statuses)
        {
            var now = Application.DateTimeProvider.Now;

            var list = statuses.Select(x => new InteractionStatusLog
            {
                Id = Guid.NewGuid().ToString(),
                Status = x.Status,
                StartedOn = now,
                InteractionId = x.Id,
                StatusDescription = x.StatusDescription
            });

            GetRepository<InteractionStatusLog>().CreateHandled(list);
            return SaveChangesAsync();
        }

        public IQueryable<TInteraction> GetInitQueryToSend<TInteraction>() where TInteraction : Interaction, new()
        {
            var dateNow = Application.DateTimeProvider.Now;

            return Query<TInteraction>().Where(x => !x.SendNow && x.SendOn >= dateNow || x.SendNow);
        }
    }
}