using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models;

[Table("Map_Statistics")]
public class DbMapStatistic
{
    [Key]
    public int MapStatisticId { get; set; }

    public DbMap Map { get; set; }

    public int MapId { get; set; }

    public int NumberOfPlays { get; set; }

    public int Cooldown { get; set; }

    public DateTime LastPlayed { get; set; }

    public int AmountSkipped { get; set; }
}
