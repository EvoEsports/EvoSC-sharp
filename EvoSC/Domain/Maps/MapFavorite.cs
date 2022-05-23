using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Players;

namespace EvoSC.Domain.Maps
{
    [Table("Player_MapFavorites")]
    public class MapFavorite
    {
        [Key]
        public int Id { get; set; }

        public Player Player { get; set; }

        public Map Map { get; set; }
    }
}
