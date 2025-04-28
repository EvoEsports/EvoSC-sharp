namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

public interface IChatStateMessage : IPlayerStateMessage
{
    public string Message { get; set; }
}
