using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Players;

namespace EvoSC.Domain.Maps
{
    [Table("Maps")]
    public class Map : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string Uid { get; set; }
        public string FilePath { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public int ManiaExchangeId { get; set; }
        public DateTime ManiaExchangeVersion { get; set; }

        public Player Player { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<MapFavorite> FavoritedMaps { get; set; }
        public IEnumerable<PersonalBest> PersonalBests { get; set; }
        public IEnumerable<MapRecord> MapRecords { get; set; }
        public IEnumerable<MapKarma> MapKarmas { get; set; }

        public MapStatistic MapStatistic { get; set; }
    }
}
