using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        
        [ForeignKey("Player")]
        public string PlayerId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
