using System.Drawing;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.ServerUtils;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Remote;

public partial class ServerClient
{
    private TextFormatter MakeInfoMessage(string text) =>
        new TextFormatter()
            .AddText(Icons.Info, styling => styling.WithColor(_config.Theme.Chat.InfoColor))
            .AddText(" ")
            .AddText(text);
    
    public Task InfoMessage(string text) =>
        this.SendChatMessage(MakeInfoMessage(text));

    public Task InfoMessage(string text, IPlayer player) =>
        this.SendChatMessage(MakeInfoMessage(text), player);
}
