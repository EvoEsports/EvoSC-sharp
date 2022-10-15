using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player;

[Controller]
public class PlayerController : EvoScController
{
    private readonly ILogger<PlayerController> _logger;
    private readonly IPlayerService _players;
    
    public PlayerController(ILogger<PlayerController> logger, IPlayerService players)
    {
        _logger = logger;
        _players = players;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect, EventPriority.High)]
    public async Task OnPlayerConnect(object sender, PlayerConnectEventArgs args)
    {
        var player = await _players.GetPlayerByLogin(args.Login);

        if (player == null)
        {
            var playerServerInfo = await Context.Server.Remote.GetDetailedPlayerInfoAsync(args.Login);
            player = await _players.NewPlayer(args.Login, playerServerInfo.NickName, playerServerInfo.Path);
            
            Context.Server.Remote.ChatSendServerMessageAsync($"{player.UbisoftName} has joined for the first time.");
        }
        else
        {
            Context.Server.Remote.ChatSendServerMessageAsync($"{player.UbisoftName} has joined.");
        }

        player.LastVisit = DateTime.UtcNow;
        _players.UpdatePlayer(player);
    }
}
