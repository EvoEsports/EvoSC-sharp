using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;

public class WaypointMessage : IWaypointStateMessage
{
    public string ClientId { get; set; }
    public DateTime Timestamp { get; set; }
    public string AccountId { get; set; }
    public string NickName { get; set; }
    public int RaceTime { get; set; }
    public int CheckpointInRace { get; set; }
    public IEnumerable<int> CurrentRaceCheckpoints { get; set; }
    public bool IsEndRace { get; set; }
    public float Speed { get; set; }
}
