namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

public interface IStateMessage
{
    /// <summary>
    /// ID/Name of the client that sent this message.
    /// </summary>
    public string ClientId { get; set; }
    
    /// <summary>
    /// The time at which this message was sent.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
