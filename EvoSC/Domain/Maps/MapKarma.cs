using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Domain.Maps
{
    [Table("Map_Karma")]
    public class MapKarma
    {
        [Key]
        public int Id { get; set; }
        public int Rating { get; set; }
        public bool New { get; set; }
        
        [ForeignKey("Map")]
        public int MapId { get; set; }
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
    }
}
