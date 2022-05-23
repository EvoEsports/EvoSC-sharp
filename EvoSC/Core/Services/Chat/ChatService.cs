using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Core.Helpers;
using EvoSC.Core.Services.Players;
using EvoSC.Core.Services.UI;
using EvoSC.Domain;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Chat;
using GbxRemoteNet;
using NLog;

namespace EvoSC.Core.Services.Chat;

public class ChatService : IChatService
{
    private readonly IChatCallbacks _chatCallbacks;
    private readonly DatabaseContext _databaseContext;
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly Manialink _manialink;

    public ChatService(DatabaseContext databaseContext, GbxRemoteClient gbxRemoteClient, IChatCallbacks chatCallbacks)
    {
        _databaseContext = databaseContext;
        _gbxRemoteClient = gbxRemoteClient;
        _chatCallbacks = chatCallbacks;
        _manialink = new Manialink(_gbxRemoteClient);
    }

    public async Task ClientOnPlayerChat(int playeruid, string login, string text, bool isregisteredcmd)
    {
        var player = await PlayerService.GetPlayer(login);
        ChatCommand command = null;
        if (text.StartsWith("/"))
        {
            command = ParseChatCommand(text);
        }

        if (command == null)
        {
            _chatCallbacks.OnPlayerChat(new PlayerChatEventArgs(player, text, isregisteredcmd));
        }
        else
        {
            await HandleCommand(player, command);
        }
    }

    private async Task HandleCommand(Player player, ChatCommand chatCommand)
    {
        switch (chatCommand.CommandName)
        {
            case "show":
                {
                    await _manialink.Send(player);
                    break;
                }

            case "hide":
                {
                    await _manialink.Hide(player);
                    break;
                }
        }
    }

    private ChatCommand ParseChatCommand(string text)
    {
        var command = new ChatCommand
        {
            IsAdminCommand = false || text.StartsWith("//"),
        };
        var commandRegex = new Regex("^/{1,2}(\\w+)");
        var whitespaceRegex = new Regex("\\s+");
        var match = commandRegex.Match(text);
        if (!match.Success)
        {
            return null;
        }

        command.CommandName = match.Groups[1].Value;
        command.Arguments = whitespaceRegex.Split(text.Replace($"{match.Groups[0].Value} ", string.Empty));
        return command;
    }

    private class ChatCommand
    {
        public string CommandName { get; set; }

        public string[] Arguments { get; set; }

        public bool IsAdminCommand { get; set; }
    }
}
