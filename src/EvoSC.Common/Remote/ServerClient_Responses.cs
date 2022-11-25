using System.Drawing;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.ServerUtils;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Remote;

public partial class ServerClient
{
    private TextFormatter MakeInfoMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(_config.Theme.Chat.InfoColor))
            .AddText(" ")
            .AddText(text);

    private TextFormatter MakeSuccessMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(_config.Theme.Chat.SuccessColor))
            .AddText(" ")
            .AddText(text);
    
    private TextFormatter MakeWarningMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(_config.Theme.Chat.WarningColor))
            .AddText(" ")
            .AddText(text);
    
    private TextFormatter MakeErrorMessage(string text) =>
        new TextFormatter()
            .AddText("", styling => styling.WithColor(_config.Theme.Chat.ErrorColor))
            .AddText(" ")
            .AddText(text);

    public Task InfoMessage(string text) =>
        this.SendChatMessage(MakeInfoMessage(text));

    public Task InfoMessage(string text, IPlayer player) =>
        this.SendChatMessage(MakeInfoMessage(text), player);
    
    public Task SuccessMessage(string text) =>
        this.SendChatMessage(MakeSuccessMessage(text));

    public Task SuccessMessage(string text, IPlayer player) =>
        this.SendChatMessage(MakeSuccessMessage(text), player);
    
    public Task WarningMessage(string text) =>
        this.SendChatMessage(MakeWarningMessage(text));

    public Task WarningMessage(string text, IPlayer player) =>
        this.SendChatMessage(MakeWarningMessage(text), player);
    
    public Task ErrorMessage(string text) =>
        this.SendChatMessage(MakeErrorMessage(text));

    public Task ErrorMessage(string text, IPlayer player) =>
        this.SendChatMessage(MakeErrorMessage(text), player);
}
