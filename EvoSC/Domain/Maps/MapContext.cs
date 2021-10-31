using System.Data.Entity;

namespace EvoSC.Domain.Maps
{
    public class MapContext : DbContext
    {
        public DbSet<Map> Maps { get; set; }
        public DbSet<MapFavorite> MapFavorites { get; set; }
        public DbSet<MapKarma> MapKarmas { get; set; }
        public DbSet<MapRecord> MapRecords { get; set; }
        public DbSet<MapStatistic> MapStatistics { get; set; }
    }
}
