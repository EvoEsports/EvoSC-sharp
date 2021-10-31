using System.Data.Entity;

namespace EvoSC.Domain.Players
{
    public class PlayerContext : DbContext
    {
        public DbSet<PersonalBest> PersonalBests { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
    }
}
