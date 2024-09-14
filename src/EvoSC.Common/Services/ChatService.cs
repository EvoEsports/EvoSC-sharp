using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Util;
using EvoSC.Common.Util.TextFormatting;
using GbxRemoteNet;

namespace EvoSC.Common.Services;

public class ChatService(IServerClient server, IThemeManager themes) : IChatService
{
    public Task<bool> SendChatMessageAsync(string message) => server.Remote.ChatSendServerMessageAsync(message);

    public Task<bool> SendChatMessageAsync(TextFormatter message) => SendChatMessageAsync(message.ToString());

    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message)
    {
        var text = new TextFormatter();
        message(text);
        return SendChatMessageAsync(text);
    }

    public async Task<bool> SendChatMessageAsync(string message, params string[] accountIds)
    {
        var mc = new MultiCall();
        foreach (var accountId in accountIds)
        {
            mc.Add("ChatSendServerMessageToLogin", message, PlayerUtils.ConvertAccountIdToLogin(accountId));
        }
        
        return (await server.Remote.MultiCallAsync(mc)).All(r => r != null && (bool)r);
    }

    public Task<bool> SendChatMessageAsync(string message, params IPlayer[] players) =>
        SendChatMessageAsync(message, players.Select(p => p.AccountId).ToArray());

    public Task<bool> SendChatMessageAsync(TextFormatter message, params string[] accountIds) =>
        SendChatMessageAsync(message.ToString(), accountIds);

    public Task<bool> SendChatMessageAsync(TextFormatter message, params IPlayer[] players) =>
        SendChatMessageAsync(message.ToString(), players.Select(p => p.AccountId).ToArray());

    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message, params string[] accountIds)
    {
        var text = new TextFormatter();
        message(text);
        return SendChatMessageAsync(text, accountIds);
    }

    public Task<bool> SendChatMessageAsync(Action<TextFormatter> message, params IPlayer[] players) =>
        SendChatMessageAsync(message, players.Select(p => p.AccountId).ToArray());

    public Task InfoMessageAsync(string text) => SendChatMessageAsync(CreateInfoMessage(text));

    public Task InfoMessageAsync(TextFormatter text) => SendChatMessageAsync(CreateInfoMessage(text.ToString()));

    public Task InfoMessageAsync(Action<TextFormatter> text) => SendChatMessageAsync(CreateInfoMessage(text));

    public Task InfoMessageAsync(string text, params IPlayer[] players) => SendChatMessageAsync(CreateInfoMessage(text), players);

    public Task InfoMessageAsync(TextFormatter text, params IPlayer[] players) => SendChatMessageAsync(CreateInfoMessage(text.ToString()), players);

    public Task InfoMessageAsync(Action<TextFormatter> text, params IPlayer[] players) => SendChatMessageAsync(CreateInfoMessage(text), players);

    public Task SuccessMessageAsync(string text) => SendChatMessageAsync(CreateSuccessMessage(text));

    public Task SuccessMessageAsync(TextFormatter text) => SendChatMessageAsync(CreateSuccessMessage(text.ToString()));

    public Task SuccessMessageAsync(Action<TextFormatter> text) => SendChatMessageAsync(CreateSuccessMessage(text));

    public Task SuccessMessageAsync(string text, params IPlayer[] players) => SendChatMessageAsync(CreateSuccessMessage(text), players);

    public Task SuccessMessageAsync(TextFormatter text, params IPlayer[] players) => SendChatMessageAsync(CreateSuccessMessage(text.ToString()), players);

    public Task SuccessMessageAsync(Action<TextFormatter> text, params IPlayer[] players) => SendChatMessageAsync(CreateSuccessMessage(text), players);

    public Task WarningMessageAsync(string text) => SendChatMessageAsync(CreateWarningMessage(text));

    public Task WarningMessageAsync(TextFormatter text) => SendChatMessageAsync(CreateWarningMessage(text.ToString()));

    public Task WarningMessageAsync(Action<TextFormatter> text) => SendChatMessageAsync(CreateWarningMessage(text));

    public Task WarningMessageAsync(string text, params IPlayer[] players) => SendChatMessageAsync(CreateWarningMessage(text), players);

    public Task WarningMessageAsync(TextFormatter text, params IPlayer[] players) => SendChatMessageAsync(CreateWarningMessage(text.ToString()), players);

    public Task WarningMessageAsync(Action<TextFormatter> text, params IPlayer[] players) => SendChatMessageAsync(CreateWarningMessage(text), players);

    public Task ErrorMessageAsync(string text) => SendChatMessageAsync(CreateErrorMessage(text));

    public Task ErrorMessageAsync(TextFormatter text) => SendChatMessageAsync(CreateErrorMessage(text.ToString()));

    public Task ErrorMessageAsync(Action<TextFormatter> text) => SendChatMessageAsync(CreateErrorMessage(text));

    public Task ErrorMessageAsync(string text, params IPlayer[] players) => SendChatMessageAsync(CreateErrorMessage(text), players);

    public Task ErrorMessageAsync(TextFormatter text, params IPlayer[] players) => SendChatMessageAsync(CreateErrorMessage(text.ToString()), players);

    public Task ErrorMessageAsync(Action<TextFormatter> text, params IPlayer[] players) => SendChatMessageAsync(CreateErrorMessage(text), players);

    private string CreateInfoMessage(string message) =>
        new TextFormatter()
            .AddText(GameIcons.Icons.ExclamationCircle, s => s
                .WithColor(new TextColor(themes.Theme.Chat_Info)))
            .AddText(" ")
            .AddText(message)
            .ToString();

    private string CreateInfoMessage(Action<TextFormatter> message) => CreateInfoMessage(CallMessageAction(message));
    
    private string CreateSuccessMessage(string message) =>
        new TextFormatter()
            .AddText(GameIcons.Icons.CheckCircle, s => s
                .WithColor(new TextColor(themes.Theme.Chat_Success)))
            .AddText(" ")
            .AddText(message)
            .ToString();
    
    private string CreateSuccessMessage(Action<TextFormatter> message) => CreateSuccessMessage(CallMessageAction(message));
    
    private string CreateWarningMessage(string message) =>
        new TextFormatter()
            .AddText(GameIcons.Icons.ExclamationTriangle, s => s
                .WithColor(new TextColor(themes.Theme.Chat_Warning)))
            .AddText(" ")
            .AddText(message)
            .ToString();
    
    private string CreateWarningMessage(Action<TextFormatter> message) => CreateWarningMessage(CallMessageAction(message));
    
    private string CreateErrorMessage(string message) =>
        new TextFormatter()
            .AddText(GameIcons.Icons.TimesCircle, s => s
                .WithColor(new TextColor(themes.Theme.Chat_Danger)))
            .AddText(" ")
            .AddText(message)
            .ToString();
    
    
    private string CreateErrorMessage(Action<TextFormatter> message) => CreateErrorMessage(CallMessageAction(message));

    private string CallMessageAction(Action<TextFormatter> message)
    {
        var text = new TextFormatter();
        message(text);
        return text.ToString();
    }
}
