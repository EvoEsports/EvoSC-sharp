using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player;

[Controller]
public class PlayerController : EvoScController
{
    private readonly ILogger<PlayerController> _logger;
    private readonly IPlayerService _players;
    private readonly IServerClient _server;
    
    public PlayerController(ILogger<PlayerController> logger, IPlayerService players, IServerClient server)
    {
        _logger = logger;
        _players = players;
        _server = server;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnect(object sender, PlayerConnectEventArgs args) => UpdateAndGreetPlayer(args.Login);
    
    private async Task UpdateAndGreetPlayer(string login)
    {
        var player = await _players.GetPlayerByLogin(login);

        if (player == null)
        {
            var playerServerInfo = await _server.Remote.GetDetailedPlayerInfoAsync(login);
            player = await _players.NewPlayer(login, playerServerInfo.NickName, playerServerInfo.Path);
            
            await _server.SendChatMessage($"{player.UbisoftName} has joined for the first time.");
        }
        else
        {
            await _server.SendChatMessage($"{player.UbisoftName} has joined.");
        }

        player.LastVisit = DateTime.UtcNow;
        _players.UpdatePlayer(player);
    }
}
