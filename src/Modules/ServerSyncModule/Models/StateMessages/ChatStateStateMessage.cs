using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;

public class ChatStateStateMessage : IChatStateStateMessage
{
    public string ClientId { get; set; }
    public required DateTime Timestamp { get; set; }
    public string NickName { get; set; }
    public string AccountId { get; set; }
    public string Message { get; set; }
}
