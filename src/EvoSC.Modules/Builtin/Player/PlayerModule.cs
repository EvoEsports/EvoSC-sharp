using EvoSC.Common.Config.Models;
using EvoSC.Common.Database;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Modules.Attributes;
using GbxRemoteNet;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Builtin.Player;

[Module]
public class PlayerModule : EvoScModule
{
    private readonly EvoScDb _db;
    private readonly ServerClient _server;
    
    public PlayerModule(EvoScDb db, ServerClient server, EventManager events)
    {
        _db = db;
        _server = server;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect, EventPriority.High)]
    public async Task OnPlayerConnect(object sender, PlayerConnectEventArgs args)
    {
        var players = await _db.QueryAsync<DbPlayer>("select * from players where Login=@Login", new {Login = args.Login});
        var player = players.FirstOrDefault();

        if (player == null)
        {
            var playerInfo = await _server.Remote.GetDetailedPlayerInfoAsync(args.Login);

            player = new DbPlayer
            {
                Login = args.Login,
                UbisoftName = playerInfo.NickName,
                Zone = playerInfo.Path,
                LastVisit = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _db.Players.InsertAsync(player);
        }

        _server.Remote.ChatSendServerMessageAsync($"{player.UbisoftName} has joined.");
    }
}
