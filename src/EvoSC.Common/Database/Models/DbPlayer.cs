using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models;

public class DbPlayer
{
    /// <summary>
    /// Database ID of the player.
    /// </summary>
    [Key]
    public long Id { get; set; }
    /// <summary>
    /// Player's account login, unique and can't change.
    /// </summary>
    public string Login { get; set; }
    /// <summary>
    /// The known ubisoft name of the player, may change.
    /// </summary>
    public string UbisoftName { get; set; }
    /// <summary>
    /// The zone/location path of a player.
    /// </summary>
    public string Zone { get; set; }
    /// <summary>
    /// Time at which the player was last seen on the server.
    /// </summary>
    public DateTime LastVisit { get; set; }
    /// <summary>
    /// Essentially the time at which the player first appeared on the server.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Time at which the player was updated the last time in the database.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
