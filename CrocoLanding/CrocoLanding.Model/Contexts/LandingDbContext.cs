using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CrocoShop.Model.Contexts
{
    public class LandingDbContext : DbContext
    {
        public const string ServerConnection = "ServerConnection";

        public const string LocalConnection = "DefaultConnection";

#if DEBUG
        public static string ConnectionString => LocalConnection;
#else
        public static string ConnectionString => ServerConnection;
#endif

        #region Конструкторы
        public LandingDbContext(DbContextOptions<LandingDbContext> options) : base(options)
        {
        }
        #endregion

        public static LandingDbContext Create(IConfiguration configuration)
        {
            return Create(configuration.GetConnectionString(ConnectionString));
        }

        public static LandingDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LandingDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LandingDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}