namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

public interface IWaypointStateMessage : IPlayerStateMessage
{
    /// <summary>
    /// Race time in milliseconds
    /// </summary>
    public int RaceTime { get; set; }

    /// <summary>
    /// Checkpoints in race
    /// </summary>
    public int CheckpointInRace { get; set; }

    /// <summary>
    /// Current checkpoints in race
    /// </summary>
    public IEnumerable<int> CurrentRaceCheckpoints { get; set; }

    /// <summary>
    /// Whether its the end of the race
    /// </summary>
    public bool IsEndRace { get; set; }

    /// <summary>
    /// Speed of the player through the checkpoint
    /// </summary>
    public float Speed { get; set; }
}
