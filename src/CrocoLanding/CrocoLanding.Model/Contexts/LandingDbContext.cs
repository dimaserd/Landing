using CrocoLanding.Model.Entities.Ecc;
using Microsoft.EntityFrameworkCore;

namespace CrocoLanding.Model.Contexts
{
    public partial class LandingDbContext : DbContext
    {
        public LandingDbContext(DbContextOptions<LandingDbContext> options) : base(options)
        {
        }
        
        public DbSet<CallBackRequest> CallBackRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}