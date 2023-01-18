using EvoSC.Common.Interfaces.Models;
using RepoDb.Attributes;
using SqlKata;

namespace EvoSC.Common.Database.Models.Player;

[Map("Players")]
public class DbPlayer : IPlayer
{
    /// <summary>
    /// Database ID of the player.
    /// </summary>
    [Key]
    public long Id { get; set; }
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

    public string AccountId { get; set; }
    public string NickName { get; set; }
    public string UbisoftName { get; set; }
    public string Zone { get; set; }
    
    [Ignore]
    public IEnumerable<IGroup> Groups { get; set; }
}
