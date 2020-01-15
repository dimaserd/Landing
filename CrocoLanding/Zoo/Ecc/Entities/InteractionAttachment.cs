using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Croco.Core.Model.Entities.Application;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Zoo.Entities.Messaging
{
    public class InteractionAttachment<TInteraction, TFile> where TInteraction : class where TFile : DbFileIntId
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey(nameof(Interaction))]
        public string InteractionId { get; set; }

        [JsonIgnore]
        public virtual TInteraction Interaction { get; set; }

        
        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(File))]
        public int FileId { get; set; }

        [JsonIgnore]
        public virtual TFile File { get; set; }

        public static void OnModelCreating<T>(ModelBuilder modelBuilder) where T : InteractionAttachment<TInteraction, TFile>
        {
            modelBuilder.Entity<T>()
                .HasKey(p => new { p.InteractionId, p.FileId,  });
        }
    }
}
