using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Interfaces.Services;

public interface IChatService
{
    /// <summary>
    /// Send a chat message to all players.
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(string message);
    /// <summary>
    /// Send a chat message to all players.
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(TextFormatter message);
    /// <summary>
    /// Send a chat message to all players.
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message);
    /// <summary>
    /// Send a chat message to a set of players.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="accountIds">Players to send the message to.</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(string message, params string[] accountIds);
    /// <summary>
    /// Send a chat message to a set of players.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="accountIds">Players to send the message to.</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(string message, params IPlayer[] players);
    /// <summary>
    /// Send a chat message to a set of players.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="accountIds">Players to send the message to.</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(TextFormatter message, params string[] accountIds);
    /// <summary>
    /// Send a chat message to a set of players.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="accountIds">Players to send the message to.</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(TextFormatter message, params IPlayer[] players);
    /// <summary>
    /// Send a chat message to a set of players.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="accountIds">Players to send the message to.</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message, params string[] accountIds);
    /// <summary>
    /// Send a chat message to a set of players.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <param name="accountIds">Players to send the message to.</param>
    /// <returns></returns>
    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message, params IPlayer[] players);
    
    /// <summary>
    /// Send an info-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(string text);
    /// <summary>
    /// Send an info-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(TextFormatter text);
    /// <summary>
    /// Send an info-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(Action<TextFormatter> text);
    /// <summary>
    /// Send an info-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(string text, params IPlayer[] players);
    /// <summary>
    /// Send an info-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(TextFormatter text, params IPlayer[] players);
    /// <summary>
    /// Send an info-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task InfoMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
    
    /// <summary>
    /// Send a success-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(string text);/// <summary>
    /// Send a success-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(TextFormatter text);
    /// <summary>
    /// Send a success-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(Action<TextFormatter> text);
    /// <summary>
    /// Send a success-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(string text, params IPlayer[] players);
    /// <summary>
    /// Send a success-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(TextFormatter text, params IPlayer[] players);
    /// <summary>
    /// Send a success-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task SuccessMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
    
    /// <summary>
    /// Send a warning-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(string text);/// <summary>
    /// Send a warning-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(TextFormatter text);/// <summary>
    /// Send a warning-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(Action<TextFormatter> text);
    /// <summary>
    /// Send a warning-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(string text, params IPlayer[] players);
    /// <summary>
    /// Send a warning-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(TextFormatter text, params IPlayer[] players);
    /// <summary>
    /// Send a warning-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task WarningMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
    
    /// <summary>
    /// Send an error-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(string text);
    /// <summary>
    /// Send an error-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(TextFormatter text);
    /// <summary>
    /// Send an error-formatted message to everyone.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(Action<TextFormatter> text);
    /// <summary>
    /// Send an error-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(string text, params IPlayer[] players);
    /// <summary>
    /// Send an error-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(TextFormatter text, params IPlayer[] players);
    /// <summary>
    /// Send an error-formatted message to a set of players.
    /// </summary>
    /// <param name="text">Message to send.</param>
    /// <param name="players">Players to send to.</param>
    /// <returns></returns>
    public Task ErrorMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
}
