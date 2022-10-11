using System.Data.Common;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSc.Commands.Attributes;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Database;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Builtin.Player;

[Controller]
public class PlayerController : IController
{
    private readonly DbConnection _db;
    private readonly IServerClient _server;
    
    public PlayerController(DbConnection db, IServerClient server)
    {
        _db = db;
        _server = server;
    }
    
    [Subscribe(GbxRemoteEvent.PlayerConnect, EventPriority.High)]
    public async Task OnPlayerConnect(object sender, PlayerConnectEventArgs args)
    {
        var players = await _db.QueryAsync<DbPlayer>("select * from players where Login=@Login", new {Login = "'"});
        var player = players.FirstOrDefault();

        if (player == null)
        {
            var playerInfo = await _server.Remote.GetDetailedPlayerInfoAsync(args.Login);

            player = new DbPlayer
            {
                Login = args.Login,
                UbisoftName = playerInfo.NickName,
                Zone = playerInfo.Path,
                LastVisit = DateTime.UtcNow
            };

            await _db.InsertAsync(player);
        }

        _server.Remote.ChatSendServerMessageAsync($"{player.UbisoftName} has joined.");
    }
}
