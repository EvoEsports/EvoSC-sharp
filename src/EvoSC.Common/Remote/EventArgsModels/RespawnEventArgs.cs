namespace EvoSC.Common.Remote.EventArgsModels;

public class RespawnEventArgs : TimedEventArgs
{
    /// <summary>
    /// Login of player
    /// </summary>
    public required string Login { get; init; }
    /// <summary>
    ///  Webservices Account ID of the player
    /// </summary>
    public required string AccountId { get; init; }
    /// <summary>
    /// Number of respawns
    /// </summary>
    public required int NbRespawns { get; init; }
    /// <summary>
    /// Race time in milliseconds since start line
    /// </summary>
    public required int RaceTime { get; init; }
    /// <summary>
    /// Lap time in milliseconds since lap start line
    /// </summary>
    public required int LapTime { get; init; }
    /// <summary>
    /// Checkpoint number since start of the race
    /// </summary>
    public required int CheckpointInRace { get; init; }
    /// <summary>
    /// Checkpoint number since start of lap
    /// </summary>
    public required int CheckpointInLap { get; init; }
    /// <summary>
    /// Speed in m/s
    /// convert km/h multiply with 3.6
    /// convert mi/h multiply with 2.23693629
    /// </summary>
    public required float Speed { get; init; }
}
