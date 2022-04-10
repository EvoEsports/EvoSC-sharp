using System;
using EvoSC.Domain.Maps;
using EvoSC.Domain.Players;
using Microsoft.EntityFrameworkCore;

namespace EvoSC.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

        public DbSet<Map> Maps { get; set; }
        public DbSet<MapFavorite> MapFavorites { get; set; }
        public DbSet<MapKarma> MapKarmas { get; set; }
        public DbSet<MapRecord> MapRecords { get; set; }
        public DbSet<MapStatistic> MapStatistics { get; set; }
        public DbSet<PersonalBest> PersonalBests { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING") ?? "server=localhost;uid=evosc;pwd=evosc123!;database=evosc;SslMode=none";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Map>()
                .HasOne(m => m.MapStatistic)
                .WithOne(ms => ms.Map)
                .HasForeignKey<MapStatistic>(ms => ms.MapId);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.PlayerStatistic)
                .WithOne(ps => ps.Player)
                .HasForeignKey<PlayerStatistic>(ps => ps.PlayerId);
        }
    }
}
