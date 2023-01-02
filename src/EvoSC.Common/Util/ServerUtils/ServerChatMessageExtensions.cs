using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Util.ServerUtils;

public static class ServerChatMessageExtensions
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

    /// <summary>
    /// Send formatted text to the chat for a specific login.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="message">The text message to send.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, TextFormatter message) =>
        server.SendChatMessage(message.ToString());

    /// <summary>
    /// Send formatted text to the chat to a specific player.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="message">The text message to send.</param>
    /// <param name="player">The player to send the message to.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, TextFormatter message, IPlayer player) =>
        server.SendChatMessage(message.ToString(), player);
    
    /// <summary>
    /// Send a chat message to a specific user by their login.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="message">The message to send.</param>
    /// <param name="login">Login of the player to send the message to.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, TextFormatter message, string login) =>
        server.SendChatMessage(message.ToString(), login);
    
    /// <summary>
    /// Send formatted text to the chat using a builder action.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="messageBuilder">The text message to send.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, Action<TextFormatter> messageBuilder)
    {
        var message = new TextFormatter();
        messageBuilder(message);
        return server.SendChatMessage(message.ToString());
    }
    
    /// <summary>
    /// Send formatted text to the chat for a specific player using a builder action.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="messageBuilder">The text message to send.</param>
    /// <param name="player">The player to send the message to.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, Action<TextFormatter> messageBuilder, IPlayer player)
    {
        var message = new TextFormatter();
        messageBuilder(message);
        return server.SendChatMessage(message.ToString(), player);
    }
    
    /// <summary>
    /// Send formatted text to the chat for a specific login using a builder action.
    /// </summary>
    /// <param name="server"></param>
    /// <param name="messageBuilder">The text message to send.</param>
    /// <param name="login">Login of the player to send the message to.</param>
    /// <returns></returns>
    public static Task SendChatMessage(this IServerClient server, Action<TextFormatter> messageBuilder, string login)
    {
        var message = new TextFormatter();
        messageBuilder(message);
        return server.SendChatMessage(message.ToString(), login);
    }
}
