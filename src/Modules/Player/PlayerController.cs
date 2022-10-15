using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.Player;

[Controller]
public class PlayerController : EvoScController
{
    private readonly ILogger<PlayerController> _logger;
    private readonly DbConnection _db;
    
    public PlayerController(ILogger<PlayerController> logger, DbConnection db)
    {
        _logger = logger;
        _db = db;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect, EventPriority.High)]
    public async Task OnPlayerConnect(object sender, PlayerConnectEventArgs args)
    {
        var players = await _db.QueryAsync<DbPlayer>("select * from players where Login=@Login", new {Login = args.Login});
        var player = players.FirstOrDefault();

        if (player == null)
        {
            var playerInfo = await Context.Server.Remote.GetDetailedPlayerInfoAsync(args.Login);

            player = new DbPlayer
            {
                Login = args.Login,
                UbisoftName = playerInfo.NickName,
                Zone = playerInfo.Path,
                LastVisit = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.InsertAsync(player);
            Context.Server.Remote.ChatSendServerMessageAsync($"{player.UbisoftName} has joined for the first time.");
        }
        else
        {
            player.LastVisit = DateTime.UtcNow;
            player.UpdatedAt = DateTime.UtcNow;
            await _db.UpdateAsync(player);
            Context.Server.Remote.ChatSendServerMessageAsync($"{player.UbisoftName} has joined.");
        }
    }
}
