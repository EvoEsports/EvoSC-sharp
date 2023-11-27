using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.ServerUtils;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Remote;

public partial class ServerClient
{
    private TextFormatter MakeInfoMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(new TextColor(_themes.Theme.Chat_Info)))
            .AddText(" ")
            .AddText(text);

    private TextFormatter MakeSuccessMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(new TextColor(_themes.Theme.Chat_Success)))
            .AddText(" ")
            .AddText(text);
    
    private TextFormatter MakeWarningMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(new TextColor(_themes.Theme.Chat_Warning)))
            .AddText(" ")
            .AddText(text);
    
    private TextFormatter MakeErrorMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(new TextColor(_themes.Theme.Chat_Error)))
            .AddText(" ")
            .AddText(text);

    public Task InfoMessageAsync(string text) =>
        this.SendChatMessageAsync(MakeInfoMessage(text));

    public Task InfoMessageAsync(string text, IPlayer player) =>
        this.SendChatMessageAsync(MakeInfoMessage(text), player);
    
    public Task SuccessMessageAsync(string text) =>
        this.SendChatMessageAsync(MakeSuccessMessage(text));

    public Task SuccessMessageAsync(string text, IPlayer player) =>
        this.SendChatMessageAsync(MakeSuccessMessage(text), player);
    
    public Task WarningMessageAsync(string text) =>
        this.SendChatMessageAsync(MakeWarningMessage(text));

    public Task WarningMessageAsync(string text, IPlayer player) =>
        this.SendChatMessageAsync(MakeWarningMessage(text), player);
    
    public Task ErrorMessageAsync(string text) =>
        this.SendChatMessageAsync(MakeErrorMessage(text));

    public Task ErrorMessageAsync(string text, IPlayer player) =>
        this.SendChatMessageAsync(MakeErrorMessage(text), player);
}
