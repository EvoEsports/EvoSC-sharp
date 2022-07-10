using System;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Core.Helpers;
using EvoSC.Core.Services.Players;
using EvoSC.Core.Services.UI;
using EvoSC.Domain;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using NLog;

namespace EvoSC.Core.Services.Chat;

public class ChatService : IChatService
{
    private readonly IChatCallbacks _chatCallbacks;
    private readonly DatabaseContext _databaseContext;
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly IPlayerService _playerService;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly Manialink _manialink;

    public event Func<IServerServerChatMessage, Task> ServerChatMessage;

    public ChatService(DatabaseContext databaseContext, GbxRemoteClient gbxRemoteClient, IChatCallbacks chatCallbacks,
        IPlayerService playerService)
    {
        _databaseContext = databaseContext;
        _gbxRemoteClient = gbxRemoteClient;
        _chatCallbacks = chatCallbacks;
        _manialink = new Manialink(_gbxRemoteClient);
        _playerService = playerService;
    }

    public async Task ClientOnPlayerChat(int playeruid, string login, string text, bool isregisteredcmd)
    {
        var player = await _playerService.GetPlayer(login);
        var chatMessage = new ServerServerChatMessage(_gbxRemoteClient, (IServerPlayer)player, text, playeruid);

        try
        {
            await ServerChatMessage?.Invoke(chatMessage)!;
        }
        catch (Exception ex)
        {
            _logger.Error("Failed to invoke ServerChatMessage event: {Msg}", ex.Message);
        }
    }
}
