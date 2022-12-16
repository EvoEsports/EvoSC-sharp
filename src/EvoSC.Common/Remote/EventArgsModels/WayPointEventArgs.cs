namespace EvoSC.Common.Remote.EventArgsModels;

public class WayPointEventArgs : EventArgs
{
    public  int Time { get; init; }
    public string Login { get; init; }
    public string AccountId { get; init; }
    public int RaceTime { get; init; }
    public int LapTime { get; init; }
    public int CheckpointInRace { get; init; }
    public int CheckpointInLap { get; init; }
    public bool IsEndRace { get; init; }
    public bool IsEndLap { get; init; }
    public IEnumerable<int> CurrentRaceCheckpoints { get; init; }
    public IEnumerable<int> CurrentLapCheckpoints { get; init; }
    public string BlockId { get; init; }
    public float Speed { get; init; }
}
