using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Core.Helpers;
using EvoSC.Domain;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EvoSC.Core.Services.Players;

public class PlayerService : IPlayerService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private static DatabaseContext s_databaseContext;
    private static GbxRemoteClient s_gbxRemoteClient;
    private readonly IPlayerCallbacks _playerCallbacks;

    private static readonly List<IPlayer> s_connectedPlayers = new();
    
    public List<IPlayer> ConnectedPlayers => s_connectedPlayers;

    public PlayerService(
        DatabaseContext databaseContext,
        GbxRemoteClient gbxRemoteClient,
        IPlayerCallbacks playerCallbacks)
    {
        s_databaseContext = databaseContext;
        s_gbxRemoteClient = gbxRemoteClient;
        _playerCallbacks = playerCallbacks;
    }

    private async Task AddConnectedPlayer(string login)
    {
        var dbPlayer =
            await s_databaseContext.Players.FirstOrDefaultAsync(dbPlayer => dbPlayer.Login == login) ??
            await CreateDatabasePlayer(login);
            
        var player = await Player.Create(s_gbxRemoteClient, dbPlayer);
        s_connectedPlayers.Add(player);
    }
    
    public async Task AddConnectedPlayers()
    {
        var playersOnline = await s_gbxRemoteClient.GetPlayerListAsync();

        if (playersOnline == null || !playersOnline.Any())
        {
            _logger.Debug("No players connected");
            return;
        }

        foreach (var playerOnline in playersOnline)
        {
            AddConnectedPlayer(playerOnline.Login);
        }
    }
    
    public async Task ClientOnPlayerInfoChanged(SPlayerInfo playerInfo)
    {
        var player = s_connectedPlayers.FirstOrDefault(p => p.Login == playerInfo.Login);

        if (player == null)
        {
            AddConnectedPlayer(playerInfo.Login);
        }
        else
        {
            (player as Player).Update(playerInfo);
        }
    }

    public async Task ClientOnPlayerConnect(string login, bool isSpectator)
    {
        var firstConnect = false;
        var dbPlayer = await s_databaseContext.Players.FirstOrDefaultAsync(player => player.Login == login);

        await CreateDatabasePlayer(login);
        
        if (dbPlayer == null)
        {
            firstConnect = true;
            dbPlayer = await CreateDatabasePlayer(login);
        }

        var player = await Player.Create(s_gbxRemoteClient, dbPlayer);

        s_connectedPlayers.Add(player);
        _playerCallbacks.OnPlayerConnect(new PlayerConnectEventArgs(player));

        _logger.Info($"Player {player.Name} ({login}) connected to the server.");
        var msg = new ChatMessage();
        msg.SetInfo();
        msg.SetIcon(Icon.Globe);
        msg.SetMessage(
            $"Player {ChatMessage.GetHighlightedString(player.Name)} joined the server. Last Visit: {ChatMessage.GetHighlightedString(Snippets.GetRelativeTimeToNow(player.LastVisit))}");
        if (firstConnect)
        {
            msg.SetMessage(
                $"Player {ChatMessage.GetHighlightedString(player.Name)} joined the server for the first time.");
        }

        await s_gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());
        player.LastVisit = DateTime.UtcNow;
        await s_databaseContext.SaveChangesAsync();
    }

    public async Task ClientOnPlayerDisconnect(string login, string reason)
    {
        _logger.Debug("PLAYERDISCONNECT FIRED");
        var player = (Player)s_connectedPlayers.FirstOrDefault(player => player.Login == login);

        if (player == null)
        {
            _logger.Warn(
                $"A disconnecting player was not found in the connected players list. Something went wrong. Player login: {login}");
            return;
        }

        var msg = new ChatMessage();
        msg.SetMessage(
            $"Player {ChatMessage.GetHighlightedString(player.Name)} left the server. Time played: {ChatMessage.GetHighlightedString(Snippets.GetTimeSpentUntilNow(player.LastVisit))}");
        msg.SetInfo();
        msg.SetIcon(Icon.UserRemove);

        await s_gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());

        s_connectedPlayers.Remove(player);
        player.LastVisit = DateTime.UtcNow;

        s_databaseContext.Players.Update((DatabasePlayer)player);
        await s_databaseContext.SaveChangesAsync();

        _playerCallbacks.OnPlayerDisconnect(new PlayerDisconnectEventArgs(player, reason));
    }

    private static async Task<DatabasePlayer> CreateDatabasePlayer(string login)
    {
        var detailedPlayerInfo = await s_gbxRemoteClient.GetDetailedPlayerInfoAsync(login);
        var player = new DatabasePlayer
        {
            Login = detailedPlayerInfo.Login,
            UbisoftName = detailedPlayerInfo.NickName,
            Group = 0,
            Path = detailedPlayerInfo.Path,
            Banned = false,
            LastVisit = DateTime.MinValue
        };

        s_databaseContext.Players.Add(player);
        await s_databaseContext.SaveChangesAsync();
        return player;
    }

    public static async Task<IPlayer> GetPlayer(string login)
    {
        foreach (var connectedPlayer in s_connectedPlayers.Where(player => player.Login == login))
        {
            return connectedPlayer;
        }

        var dbPlayer = await CreateDatabasePlayer(login);
        
        var player = await Player.Create(s_gbxRemoteClient, dbPlayer);
        s_connectedPlayers.Add(player);
        
        return player;
    }
}
