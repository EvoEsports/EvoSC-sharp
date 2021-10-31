using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Domain.Maps
{
    [Table("Map_Statistics")]
    public class MapStatistic
    {
        [Key]
        [ForeignKey("Map")]
        public int MapId { get; set; }
        public int NumberOfPlays { get; set; }
        public int Cooldown { get; set; }
        public DateTime LastPlayed { get; set; }
        public int AmountSkipped { get; set; }
    }
}
