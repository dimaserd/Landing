using Croco.Core.Abstractions.Data.Entities.HaveId;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zoo.Core
{
    public class WebAppRequestContextLog : IHaveStringId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string RequestId { get; set; }

        public string ParentRequestId { get; set; }

        public string Uri { get; set; }

        public DateTime StartedOn { get; set; }

        public DateTime FinishedOn { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebAppRequestContextLog>().HasIndex(x => x.StartedOn).IsUnique(false);
            modelBuilder.Entity<WebAppRequestContextLog>().HasIndex(x => x.Uri).IsUnique(false);
            modelBuilder.Entity<WebAppRequestContextLog>().HasIndex(x => x.RequestId).IsUnique(false);
            modelBuilder.Entity<WebAppRequestContextLog>().HasIndex(x => x.UserId).IsUnique(false);
        }
    }
}