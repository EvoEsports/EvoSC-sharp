using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Util;

public static class ServerClientExtensions
{
    /// <summary>
    /// Send a chat message to all users.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="message">The message to send.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, string message) =>
        server.Remote.ChatSendServerMessageAsync(message);

    /// <summary>
    /// Send a chat message to a specific user.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="message">The message to send.</param>
    /// <param name="player">The player to send the message to.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, string message, IPlayer player) =>
        server.Remote.ChatSendServerMessageToLoginAsync(message, PlayerUtils.ConvertAccountIdToLogin(player.AccountId));
    
    /// <summary>
    /// Send a chat message to a specific user by their login.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="message">The message to send.</param>
    /// <param name="login">Login of the player to send the message to.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, string message, string login) =>
        server.Remote.ChatSendServerMessageToLoginAsync(message, login);
}
