namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

public interface IChatStateStateMessage : IPlayerStateMessage
{
    public string Message { get; set; }
}
