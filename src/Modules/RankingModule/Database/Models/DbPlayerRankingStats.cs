using EvoSC.Common.Database.Models.Player;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.RankingModule.Database.Models;

[Table(TableName)]
public class DbPlayerRankingStats
{
    public const string TableName = "PlayerRankingStats";

    [PrimaryKey, Identity]
    public long Id { get; set; }

    [Column]
    public long PlayerId { get; set; }

    /// <summary>
    /// Total time the player has spent on the server, in seconds.
    /// </summary>
    [Column]
    public long PlaytimeSeconds { get; set; }

    /// <summary>
    /// Number of maps the player has been ranked in at the end of the map.
    /// </summary>
    [Column]
    public int MapsPlayed { get; set; }

    /// <summary>
    /// Number of maps the player has won.
    /// </summary>
    [Column]
    public int MapsWon { get; set; }

    [Association(ThisKey = nameof(PlayerId), OtherKey = nameof(DbPlayer.Id))]
    public DbPlayer? DbPlayer { get; set; }
}
