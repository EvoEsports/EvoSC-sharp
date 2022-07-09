using EvoSC.Interfaces.Players;

namespace EvoSC.Interfaces.Messages;

public interface IServerMessage
{
    /// <summary>
    /// Player that sent the message.
    /// </summary>
    public IServerPlayer Player { get; }
    
    /// <summary>
    /// Content of the message.
    /// </summary>
    public string Content { get; }
    
    /// <summary>
    /// ID of player connected to the server.
    /// </summary>
    public int PlayerServerId { get; }
}
