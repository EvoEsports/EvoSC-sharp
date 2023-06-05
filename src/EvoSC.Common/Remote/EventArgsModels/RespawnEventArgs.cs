namespace EvoSC.Common.Remote.EventArgsModels;

public class RespawnEventArgs : TimedEventArgs
{
    public required string Login { get; init; }
    public required string AccountId { get; init; }
    public required int NbRespawns { get; init; }
    public required int RaceTime { get; init; }
    public required int LapTime { get; init; }
    public required int CheckpointInRace { get; init; }
    public required int CheckpointInLap { get; init; }
    public required float Speed { get; init; }
}
