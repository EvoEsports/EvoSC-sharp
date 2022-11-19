﻿using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player;

[Controller]
public class PlayerController : EvoScController<EventControllerContext>
{
    private readonly ILogger<PlayerController> _logger;
    private readonly IPlayerService _players;
    private readonly IServerClient _server;
    private readonly IMyPlayerService _myPlayerService;
    
    public PlayerController(ILogger<PlayerController> logger, IPlayerService players, IServerClient server, IMyPlayerService myPlayerService)
    {
        _logger = logger;
        _players = players;
        _server = server;
        _myPlayerService = myPlayerService;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnect(object sender, PlayerConnectEventArgs args) => UpdateAndGreetPlayer(args.Login);
    
    private async Task UpdateAndGreetPlayer(string login)
    {
        _myPlayerService.LogIt();
        var player = await _players.GetPlayerByLogin(login);

        if (player == null)
        {
            var playerServerInfo = await _server.Remote.GetDetailedPlayerInfoAsync(login);
            var accountId = PlayerUtils.ConvertLoginToAccountId(login);
            player = await _players.NewPlayer(accountId, playerServerInfo.NickName, playerServerInfo.Path);
            
            await _server.SendChatMessage($"{player.UbisoftName} has joined for the first time.");
        }
        else
        {
            await _server.SendChatMessage($"{player.UbisoftName} has joined.");
        }

        player.LastVisit = DateTime.UtcNow;
        await _players.UpdatePlayer(player);
    }
}
