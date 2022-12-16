namespace EvoSC.Common.Remote.EventArgsModels;

public class WayPointEventArgs : EventArgs
{
    public required int Time { get; init; }
    public required string Login { get; init; }
    public required string AccountId { get; init; }
    public required int RaceTime { get; init; }
    public required int LapTime { get; init; }
    public required int CheckpointInRace { get; init; }
    public required int CheckpointInLap { get; init; }
    public required bool IsEndRace { get; init; }
    public required bool IsEndLap { get; init; }
    public required IEnumerable<int> CurrentRaceCheckpoints { get; init; }
    public required IEnumerable<int> CurrentLapCheckpoints { get; init; }
    public required string BlockId { get; init; }
    public required float Speed { get; init; }
}
