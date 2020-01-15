using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Zoo.Ecc.Entities;

namespace Zoo.Entities.Messaging
{
    public class InteractionStatusLog<TInteraction> where TInteraction : class
    {
        public string Id { get; set; } 

        [ForeignKey(nameof(Interaction))]
        public string InteractionId { get; set; }

        public virtual TInteraction Interaction { get; set; }

        public InteractionStatus Status { get; set; }

        public DateTime StartedOn { get; set; }

        public string StatusDescription { get; set; }
    }
}