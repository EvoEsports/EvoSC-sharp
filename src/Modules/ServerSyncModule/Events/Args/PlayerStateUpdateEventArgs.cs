using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args;

public class PlayerStateUpdateEventArgs : EventArgs
{
    public required IPlayerStateUpdateMessage PlayerState { get; set; }
}
