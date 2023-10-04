using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public record ExpandedLiveRankingPosition
{
    /// <summary>
    /// The player.
    /// </summary>
    public required IOnlinePlayer Player { get; init; }
    
    /// <summary>
    /// The time at the checkpoint.
    /// </summary>
    public required int CheckpointTime { get; init; }
    
    /// <summary>
    /// The index of the checkpoint.
    /// </summary>
    public required int CheckpointIndex { get; init; }
    
    /// <summary>
    /// Whether the player retreated from the round.
    /// </summary>
    public required bool IsDnf { get; init; }
    
    /// <summary>
    /// Whether the player crossed the finish line.
    /// </summary>
    public required bool IsFinish { get; init; }
    
    /// <summary>
    /// The difference in milliseconds of the players time to the leading players time.
    /// </summary>
    public int DiffToFirstPosition { get; set; }
}
