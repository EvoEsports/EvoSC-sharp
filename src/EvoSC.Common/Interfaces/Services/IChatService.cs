using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Interfaces.Services;

public interface IChatService
{
    public Task<bool> SendChatMessageAsync(string message);
    public Task<bool> SendChatMessageAsync(TextFormatter message);
    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message);
    public Task<bool> SendChatMessageAsync(string message, params string[] accountIds);
    public Task<bool> SendChatMessageAsync(string message, params IPlayer[] players);
    public Task<bool> SendChatMessageAsync(TextFormatter message, params string[] accountIds);
    public Task<bool> SendChatMessageAsync(TextFormatter message, params IPlayer[] players);
    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message, params string[] accountIds);
    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message, params IPlayer[] players);
    
    public Task InfoMessageAsync(string text);
    public Task InfoMessageAsync(TextFormatter text);
    public Task InfoMessageAsync(Action<TextFormatter> text);
    public Task InfoMessageAsync(string text, params IPlayer[] players);
    public Task InfoMessageAsync(TextFormatter text, params IPlayer[] players);
    public Task InfoMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
    
    
    public Task SuccessMessageAsync(string text);
    public Task SuccessMessageAsync(TextFormatter text);
    public Task SuccessMessageAsync(Action<TextFormatter> text);
    public Task SuccessMessageAsync(string text, params IPlayer[] players);
    public Task SuccessMessageAsync(TextFormatter text, params IPlayer[] players);
    public Task SuccessMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
    
    
    public Task WarningMessageAsync(string text);
    public Task WarningMessageAsync(TextFormatter text);
    public Task WarningMessageAsync(Action<TextFormatter> text);
    public Task WarningMessageAsync(string text, params IPlayer[] players);
    public Task WarningMessageAsync(TextFormatter text, params IPlayer[] players);
    public Task WarningMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
    
    public Task ErrorMessageAsync(string text);
    public Task ErrorMessageAsync(TextFormatter text);
    public Task ErrorMessageAsync(Action<TextFormatter> text);
    public Task ErrorMessageAsync(string text, params IPlayer[] players);
    public Task ErrorMessageAsync(TextFormatter text, params IPlayer[] players);
    public Task ErrorMessageAsync(Action<TextFormatter> text, params IPlayer[] players);
}
