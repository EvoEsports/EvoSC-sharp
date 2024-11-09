using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.RoundRankingModule.Models;

/// <summary>
/// Model that contains a player instance and their checkpoint information.
/// This class is used to pass data to the template engine.
/// </summary>
public class CheckpointData
{
    /// <summary>
    /// The player associated with the checkpoint data.
    /// </summary>
    public required IOnlinePlayer Player { get; init; }
    
    /// <summary>
    /// The current checkpoint index the player is at.
    /// </summary>
    public required int CheckpointId { get; init; }
    
    /// <summary>
    /// The time at the current checkpoint.
    /// </summary>
    public required IRaceTime Time { get; set; }
    
    /// <summary>
    /// The time difference between the leading an current player.
    /// </summary>
    public IRaceTime? TimeDifference { get; set; }
    
    /// <summary>
    /// Whether the checkpoint is the finish.
    /// </summary>
    public required bool IsFinish { get; init; }
    
    /// <summary>
    /// Whether the players did not finish their round.
    /// </summary>
    public required bool IsDNF { get; init; }
    
    /// <summary>
    /// The gained points for the current round.
    /// </summary>
    public int GainedPoints { get; set; }
    
    /// <summary>
    /// The background color for the gained points box in the widget.
    /// </summary>
    public string? AccentColor { get; set; }

    public IRaceTime GetTimeDifferenceAbsolute(CheckpointData checkpointData)
    {
        return RaceTime.FromMilliseconds(int.Abs(this.Time.TotalMilliseconds - checkpointData.Time.TotalMilliseconds));
    }
}
