using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Players;

namespace EvoSC.Domain.Maps
{
    [Table("Map_Karma")]
    public class MapKarma
    {
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        public bool New { get; set; }
        
        public Map Map { get; set; }
        public Player Player { get; set; }
    }
}
