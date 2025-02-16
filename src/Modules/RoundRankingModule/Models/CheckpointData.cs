using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Remote.EventArgsModels;
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

    /// <summary>
    /// Returns the time string displayed in the widget.
    /// </summary>
    /// <returns></returns>
    public string? FormattedTime()
    {
        if (IsDNF)
        {
            return "DNF";
        }

        var formattedTime = (TimeDifference ?? Time).ToString();

        return formattedTime?.TrimStart('0');
    }

    /// <summary>
    /// Returns the index/gained points string in the widget.
    /// </summary>
    /// <returns></returns>
    public string IndexText()
    {
        if (IsDNF)
        {
            return GameIcons.Icons.FlagO;
        }

        return IsFinish ? GameIcons.Icons.FlagCheckered : (CheckpointId + 1).ToString();
    }

    /// <summary>
    /// Creates a race time containing the time difference of the given checkpoint data, relative to this checkpoint data.
    /// </summary>
    /// <param name="checkpointData"></param>
    /// <returns></returns>
    public IRaceTime GetTimeDifferenceAbsolute(CheckpointData checkpointData)
    {
        return RaceTime.FromMilliseconds(int.Abs(this.Time.TotalMilliseconds - checkpointData.Time.TotalMilliseconds));
    }

    /// <summary>
    /// Creates a CheckpointData instance from WayPointEventArgs.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="args"></param>
    /// <param name="isDnf"></param>
    /// <returns></returns>
    public static CheckpointData FromWaypointEventArgs(IOnlinePlayer player, WayPointEventArgs args)
    {
        return new CheckpointData
        {
            Player = player,
            CheckpointId = args.CheckpointInLap,
            Time = RaceTime.FromMilliseconds(args.LapTime),
            IsFinish = args.IsEndLap,
            IsDNF = false
        };
    }

    /// <summary>
    /// Creates a CheckpointData instance for DNFs.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static CheckpointData CreateDnfEntry(IOnlinePlayer player)
    {
        return new CheckpointData
        {
            Player = player,
            CheckpointId = -1,
            Time = RaceTime.FromMilliseconds(0),
            IsFinish = false,
            IsDNF = true
        };
    }
}
