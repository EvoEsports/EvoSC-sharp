using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models.Player;

[Table("PlayerStatistics")]
public class DbPlayerStatistic
{
    [Key]
    public int PlayerStatisticId { get; set; }

    public DbPlayer Player { get; set; }

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
