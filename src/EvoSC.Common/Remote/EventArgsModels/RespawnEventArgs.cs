namespace EvoSC.Common.Remote.EventArgsModels;

public class RespawnEventArgs : EventArgs
{
    public int Time { get; init; }
    public string Login { get; init; }
    public string AccountId { get; init; }
    public int NbRespawns { get; init; }
    public int RaceTime { get; init; }
    public int LapTime { get; init; }
    public int CheckpointInRace { get; init; }
    public int CheckpointInLap { get; init; }
    public float Speed { get; init; }
}
