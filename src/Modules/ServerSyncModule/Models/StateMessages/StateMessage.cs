using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;

public class StateMessage : IStateMessage
{
    public string ClientId { get; set; }
    public DateTime Timestamp { get; set; }
}
