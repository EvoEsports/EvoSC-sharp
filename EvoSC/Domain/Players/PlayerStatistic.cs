using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EvoSC.Domain.Players
{
    [Table("Player_Statistics")]
    public class PlayerStatistic
    {
        [Key] public int PlayerStatisticId { get; set; }

        public DatabasePlayer Player { get; set; }
        public int PlayerId { get; set; }

        public int Visits { get; set; }

        public int PlayTime { get; set; }

        public int Finishes { get; set; }

        public int LocalRecords { get; set; }

        public int Ratings { get; set; }

        public int Wins { get; set; }

        public int Score { get; set; }

        public int Rank { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
