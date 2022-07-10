using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Maps;
using EvoSC.Interfaces.Players;
using McMaster.NETCore.Plugins;

namespace EvoSC.Domain.Players
{
    [Table("Players")]
    public class DatabasePlayer : IPlayer
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }
        
        [NotMapped]
        public string Name => UbisoftName;

        public string Nickname { get; set; }

        public string UbisoftName { get; set; }

        // TODO: Needs to be changed to correct group
        public int Group { get; set; }

        public string Path { get; set; }

        public bool Banned { get; set; }

        public DateTime LastVisit { get; set; }

        public PlayerStatistic PlayerStatistic { get; set; }

        public IEnumerable<PersonalBest> PersonalBests { get; set; }

        public IEnumerable<MapRecord> MapRecords { get; set; }

        public IEnumerable<MapKarma> MapKarmas { get; set; }
        
        public DatabasePlayer(){}

        public DatabasePlayer(DatabasePlayer player)
        {
            Id = player.Id;
            Login = player.Login;
            Nickname = player.Nickname;
            UbisoftName = player.UbisoftName;
            Group = player.Group;
            Path = player.Path;
            Banned = player.Banned;
            LastVisit = player.LastVisit;
            PlayerStatistic = player.PlayerStatistic;
            PersonalBests = player.PersonalBests;
            MapRecords = player.MapRecords;
            MapKarmas = player.MapKarmas;
        }
    }
}
