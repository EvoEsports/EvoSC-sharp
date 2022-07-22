using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Players;

namespace EvoSC.Domain.Maps
{
    [Table("Map_Records")]
    public class MapRecord : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public int Score { get; set; }

        public int Rank { get; set; }

        public string Checkpoints { get; set; }

        public DatabasePlayer DatabasePlayer { get; set; }

        public Map Map { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
