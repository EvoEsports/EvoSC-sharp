using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvoSC.Domain.Players
{
    [Table("Players")]
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Nickname { get; set; }
        public string UbisoftName { get; set; }
        public int Group { get; set; }
        public string Path { get; set; }
        public bool Banned { get; set; }
        public DateTime LastVisit { get; set; }
    }
}
