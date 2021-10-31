using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Domain.Maps
{
    [Table("Player_MapFavorites")]
    public class MapFavorite
    {
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        
        [ForeignKey("Map")]
        public int MapId { get; set; }
    }
}
