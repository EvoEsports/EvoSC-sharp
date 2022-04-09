using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EvoSC.Domain;
using EvoSC.Interfaces.Player;
using GbxRemoteNet;

namespace EvoSC.Core.Services.Player;

public class PlayerService : IPlayerService
{
    private readonly DatabaseContext _databaseContext;
    private readonly GbxRemoteClient _gbxRemoteClient;

    private List<Domain.Players.Player> _connectedPlayers = new ();

    public PlayerService(DatabaseContext databaseContext, GbxRemoteClient gbxRemoteClient)
    {
        _databaseContext = databaseContext;
        _gbxRemoteClient = gbxRemoteClient;
    }

    public async Task ClientOnPlayerConnect(string login, bool isspectator)
    {
        var player = await _databaseContext.Players.FirstOrDefaultAsync(player => player.Login == login);

        if (player == null)
        {
            player = await CreatePlayer(login, isspectator);
        }
        
        _connectedPlayers.Add(player);
    }
    
    public Task ClientOnPlayerDisconnect(string login, string reason)
    {
        throw new NotImplementedException();
    }

    private async Task<Domain.Players.Player> CreatePlayer(string login, bool isspectator)
    {
        var playerDetailedInfo = await _gbxRemoteClient.GetDetailedPlayerInfoAsync(login);
        var player = new Domain.Players.Player
        {
            Login = login,
            UbisoftName = playerDetailedInfo.NickName,
            Group = 0,
            Path = playerDetailedInfo.Path,
            Banned = false,
            LastVisit = DateTime.UtcNow
        };

        _databaseContext.Players.Add(player);
        return player;
    }
}
