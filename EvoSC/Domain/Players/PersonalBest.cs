using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Domain.Players
{
    [Table("Player_PersonalBests")]
    public class PersonalBest : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int Score { get; set; }
        public int Checkpoints { get; set; }
        
        [ForeignKey("Player")]
        [Column(Order = 1)]
        public int PlayerId { get; set; }
        [ForeignKey("Map")]
        [Column(Order = 2)]
        public int MapId { get; set; }
        
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
