namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Events;

public enum ServerSyncEvents
{
    /// <summary>
    /// Always triggered whenever a message is received.
    /// </summary>
    PlayerStateUpdate,
    
    /// <summary>
    /// When a new chat message has been sent from one of the other servers.
    /// </summary>
    ChatMessage,
    
    /// <summary>
    /// When the map was finished on a server.
    /// </summary>
    MapFinished
}
