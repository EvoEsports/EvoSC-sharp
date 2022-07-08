using System.Threading.Tasks;
using EvoSC.Core.Helpers;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Players;

namespace EvoSC.Interfaces.Chat;

public interface IServerServerChatMessage : IServerMessage
{
    /// <summary>
    /// Whether the message is an attempted command.
    /// </summary>
    public bool IsCommand { get; }
    /// <summary>
    /// Whether the message came from the server itself.
    /// </summary>
    public bool IsServer { get; }


    /// <summary>
    /// Reply to the player that sent the message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task ReplyAsync(string message);

    /// <summary>
    /// Reply to the player that sent the message.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Task ReplyAsync(ChatMessage message);
}
