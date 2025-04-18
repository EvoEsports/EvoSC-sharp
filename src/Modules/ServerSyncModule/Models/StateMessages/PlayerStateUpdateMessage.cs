using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;

public class PlayerStateUpdateMessage : IPlayerStateUpdateMessage
{
    public string ClientId { get; set; }
    public DateTime Timestamp { get; set; }
    public string AccountId { get; set; }
    public string NickName { get; set; }
    public IEnumerable<long> Scores { get; set; }
    public long Position { get; set; }
    public IEnumerable<long> CheckpointScores { get; set; }
    public IEnumerable<long> Times { get; set; }
}
