using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args;

public class ChatStateMessageEventArgs : EventArgs
{
    public required IChatStateMessage ChatMessage { get; init; }
}
