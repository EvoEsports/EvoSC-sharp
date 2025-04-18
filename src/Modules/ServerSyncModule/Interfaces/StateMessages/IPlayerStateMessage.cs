namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

public interface IPlayerStateMessage : IStateMessage
{
    /// <summary>
    /// Account ID of the player.
    /// </summary>
    public string AccountId { get; set; }
    
    /// <summary>
    /// Nickname of the player. This can be the set name in EvoSC or default ubisoft name.
    /// </summary>
    public string NickName { get; set; }
}
