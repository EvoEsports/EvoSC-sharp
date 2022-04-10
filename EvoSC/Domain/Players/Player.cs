using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Maps;

namespace EvoSC.Domain.Players
{
    [Table("Players")]
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
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
        
        [NotMapped]
        public bool IsSpectator { get; set; }
    }
}
