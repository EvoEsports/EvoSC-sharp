using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Maps;

namespace EvoSC.Domain.Players
{
    [Table("Player_PersonalBests")]
    public class PersonalBest : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public int Score { get; set; }

        public int Checkpoints { get; set; }

        public Player Player { get; set; }

        public Map Map { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
