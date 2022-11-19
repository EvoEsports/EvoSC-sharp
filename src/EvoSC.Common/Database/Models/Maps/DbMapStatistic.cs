using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models.Maps;

[Table("MapStatistics")]
public class DbMapStatistic
{
    [Key]
    public long Id { get; set; }

    public DbMap Map { get; set; }

    public int TimesPlayed { get; set; }

    public int Cooldown { get; set; }

    public DateTime LastPlayed { get; set; }

    public int AmountSkipped { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
