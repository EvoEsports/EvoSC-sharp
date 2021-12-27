using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Players;

namespace EvoSC.Domain.Maps
{
    [Table("Player_MapFavorites")]
    public class MapFavorite
    {
        public Player Player { get; set; }
        public Map Map { get; set; }
    }
}
