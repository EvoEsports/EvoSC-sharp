namespace EvoSC.Common.Remote.EventArgsModels;

public class WayPointEventArgs : TimedEventArgs
{
    /// <summary>
    /// Login of player
    /// </summary>
    public required string Login { get; init; }
    /// <summary>
    /// WebServices account ID of the player
    /// </summary>
    public required string AccountId { get; init; }
    /// <summary>
    /// Total race time since start line, integer in milliseconds
    /// </summary>
    public required int RaceTime { get; init; }
    /// <summary>
    /// Total lap time since start of the lap, integer in milliseconds
    /// </summary>
    public required int LapTime { get; init; }
    /// <summary>
    /// Number of checkpoint in the race, since start 
    /// </summary>
    public required int CheckpointInRace { get; init; }
    /// <summary>
    /// Number of checkpoint in the lap, since start of the lap
    /// </summary>
    public required int CheckpointInLap { get; init; }
    /// <summary>
    /// Is the event finish line
    /// </summary>
    public required bool IsEndRace { get; init; }
    /// <summary>
    /// Is the event lap finish line
    /// </summary>
    public required bool IsEndLap { get; init; }
    /// <summary>
    /// List of current race checkpoints
    /// </summary>
    public required IEnumerable<int> CurrentRaceCheckpoints { get; init; }
    /// <summary>
    /// List of current lap checkpoints
    /// </summary>
    public required IEnumerable<int> CurrentLapCheckpoints { get; init; }
    /// <summary>
    /// ManiaScript ID of the block
    /// </summary>
    public required string BlockId { get; init; }
    /// <summary>
    /// Speed in m/s
    /// convert km/h multiply with 3.6
    /// convert mi/h multiply with 2.23693629
    /// </summary>
    public required float Speed { get; init; }
}
