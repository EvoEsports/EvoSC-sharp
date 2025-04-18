using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Events.Args;

public class WaypointUpdateEventArgs : EventArgs
{
    public required IWaypointStateMessage WaypointState { get; set; }
}
