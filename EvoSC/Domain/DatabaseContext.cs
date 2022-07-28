using System;
using EvoSC.Core.Helpers;
using EvoSC.Domain.Groups;
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

        public DbSet<DatabasePlayer> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING") ??
                                   "server=localhost;uid=evosc;password=evosc123!;database=evosc";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DatabasePlayer>(e =>
            {
                e.HasIndex(p => p.Login).IsUnique();
                e.HasOne(p => p.Group);

                e.HasOne(p => p.PlayerStatistic)
                    .WithOne(ps => ps.Player)
                    .HasForeignKey<PlayerStatistic>(ps => ps.PlayerId);
            });

            modelBuilder.Entity<Map>()
                .HasOne(m => m.MapStatistic)
                .WithOne(ms => ms.Map)
                .HasForeignKey<MapStatistic>(ms => ms.MapId);

            modelBuilder.Entity<Permission>(e =>
            {
                e.HasIndex(p => p.Name).IsUnique();
            });

            modelBuilder.Entity<Group>(e =>
            {
                e.HasIndex(p => p.Name).IsUnique();

                e.HasMany(p => p.Permissions).WithMany(p => p.Groups);

                e.HasData(new Group[]
                {
                    new()
                    {
                        Id = (int)SystemGroups.MasterAdmin,
                        Name = "MasterAdmin",
                        Color = "F00",
                        Prefix = Icon.Shield,
                        SystemGroup = true
                    },
                    new()
                    {
                        Id = (int)SystemGroups.Player,
                        Name = "Player",
                        Color = "FFF",
                        Prefix = Icon.User,
                        SystemGroup = true
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Admin",
                        Color = "F55",
                        Prefix = Icon.Shield,
                        SystemGroup = false
                    },
                });
            });
        }
    }
}
