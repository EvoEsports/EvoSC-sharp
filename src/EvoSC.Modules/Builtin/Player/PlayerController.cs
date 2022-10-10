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
    private readonly EvoScDb _db;
    private readonly IServerClient _server;
    
    public PlayerController(EvoScDb db, IServerClient server)
    {
        _db = db;
        _server = server;
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerConnect(object sender, PlayerChatEventArgs args)
    {
        Console.WriteLine("player Chatting");
    }
    
    /* [Command("kick", "Kick a player.")]
    public Task KickPlayer(string login) => _server.Remote.KickAsync(login);

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
    } */
}
