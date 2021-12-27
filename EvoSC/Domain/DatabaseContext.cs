using System.Data.Entity;
using EvoSC.Domain.Maps;
using EvoSC.Domain.Players;

namespace EvoSC.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DatabaseConnectionString")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DatabaseContext>());
        }
        
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapFavorite> MapFavorites { get; set; }
        public DbSet<MapKarma> MapKarmas { get; set; }
        public DbSet<MapRecord> MapRecords { get; set; }
        public DbSet<MapStatistic> MapStatistics { get; set; }
        public DbSet<PersonalBest> PersonalBests { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
    }
}
