using System;
using Zoo.Ecc.Entities;

namespace Zoo.Ecc.Models
{
    public class InteractionStatusLogModel
    {
        public InteractionStatus Status { get; set; }

        public DateTime StartedOn { get; set; }

        /// <summary>
        /// Описание статуса взаимодействия (может содержать Exception StackTrace)
        /// </summary>
        public string StatusDescription { get; set; }
    }
}