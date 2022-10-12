using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSc.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Database;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Builtin.Player;

[Controller]
public class PlayerController : EvoScController
{
    private readonly ILogger<PlayerController> _logger;
    
    public PlayerController(ILogger<PlayerController> logger)
    {
        _logger = logger;
    }

    [Subscribe(GbxRemoteEvent.PlayerChat, EventPriority.High)]
    public async Task OnPlayerConnect(object sender, PlayerChatEventArgs args)
    {
        _logger.LogInformation("player {Login} said: {Msg}", args.Login, args.Text);

        /* var players = await _db.QueryAsync<DbPlayer>("select * from players where Login=@Login", new {Login = "'"});
        var player = players.FirstOrDefault();

        if (player == null)
        {
            var playerInfo = await Context.Server.Remote.GetDetailedPlayerInfoAsync(args.Login);

            player = new DbPlayer
            {
                Login = args.Login,
                UbisoftName = playerInfo.NickName,
                Zone = playerInfo.Path,
                LastVisit = DateTime.UtcNow
            };

            await _db.InsertAsync(player);
        }

        Context.Server.Remote.ChatSendServerMessageAsync($"{player.UbisoftName} has joined."); */
    }
}
