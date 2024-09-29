namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;

public interface IServerState
{
    /// <summary>
    /// The time at which this state change occurred.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
