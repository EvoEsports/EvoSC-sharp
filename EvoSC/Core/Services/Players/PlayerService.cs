using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Core.Helpers;
using EvoSC.Domain;
using EvoSC.Domain.Groups;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace EvoSC.Core.Services.Players;

public class PlayerService : IPlayerService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly IPlayerCallbacks _playerCallbacks;
    private readonly IServiceProvider _serviceProvider;

    private readonly List<IPlayer> _connectedPlayers = new();

    public List<IPlayer> ConnectedPlayers => _connectedPlayers;

    public PlayerService(
        GbxRemoteClient gbxRemoteClient,
        IPlayerCallbacks playerCallbacks, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _gbxRemoteClient = gbxRemoteClient;
        _playerCallbacks = playerCallbacks;
    }

    private async Task AddConnectedPlayer(string login)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var dbPlayer =
            await dbContext.Players.FirstOrDefaultAsync(dbPlayer => dbPlayer.Login == login) ??
            await CreateDatabasePlayer(login);

        var player = await Player.Create(_gbxRemoteClient, dbContext, dbPlayer);
        _connectedPlayers.Add(player);
    }

    public async Task AddConnectedPlayers()
    {
        var playersOnline = await _gbxRemoteClient.GetPlayerListAsync();

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
        var player = _connectedPlayers.FirstOrDefault(p => p.Login == playerInfo.Login);

        if (player == null)
        {
            AddConnectedPlayer(playerInfo.Login);
        }
        else
        {
            (player as Player).Update(playerInfo);
        }
    }

    // todo: trigger this with an player connect event so that we can pass a IServerPlayer instance.
    public async Task ClientOnPlayerConnect(string login, bool isSpectator)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var firstConnect = false;
        var dbPlayer = await dbContext.Players.FirstOrDefaultAsync(player => player.Login == login);

        if (dbPlayer == null)
        {
            firstConnect = true;
            dbPlayer = await CreateDatabasePlayer(login);
        }

        var player = await Player.Create(_gbxRemoteClient, dbContext, dbPlayer);
        player.LastVisit = DateTime.UtcNow;

        _connectedPlayers.Add(player);
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

        await _gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());
        dbContext.Players.Update(player);
        await dbContext.SaveChangesAsync();
    }

    public async Task ClientOnPlayerDisconnect(string login, string reason)
    {
        _logger.Debug("PLAYERDISCONNECT FIRED");

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var player = (Player)_connectedPlayers.FirstOrDefault(player => player.Login == login);

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

        await _gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());

        _connectedPlayers.Remove(player);
        player.LastVisit = DateTime.UtcNow;

        dbContext.Players.Update((DatabasePlayer)player);
        await dbContext.SaveChangesAsync();

        _playerCallbacks.OnPlayerDisconnect(new PlayerDisconnectEventArgs(player, reason));
    }

    private async Task<DatabasePlayer> CreateDatabasePlayer(string login)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        var detailedPlayerInfo = await _gbxRemoteClient.GetDetailedPlayerInfoAsync(login);
        var playerGroup = await dbContext.Groups.FirstOrDefaultAsync(g => g.Id == SystemGroups.Player);

        var playerStats = new PlayerStatistic
        {
            Visits = 1,
            PlayTime = 0,
            Finishes = 0,
            LocalRecords = 0,
            Ratings = 0,
            Wins = 0,
            Score = 0,
            Rank = 0
        };

        var player = new DatabasePlayer
        {
            Login = detailedPlayerInfo.Login,
            UbisoftName = detailedPlayerInfo.NickName,
            Nickname = detailedPlayerInfo.NickName,
            Group = playerGroup,
            Path = detailedPlayerInfo.Path,
            Banned = false,
            LastVisit = DateTime.MinValue,
            PlayerStatistic = playerStats
        };

        await dbContext.Players.AddAsync(player);
        await dbContext.SaveChangesAsync();

        return player;
    }

    public async Task<IPlayer> GetPlayer(string login)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

        foreach (var connectedPlayer in _connectedPlayers.Where(player => player.Login == login))
        {
            return connectedPlayer;
        }

        var dbPlayer = await dbContext.Players.FirstOrDefaultAsync(p => p.Login == login);

        if (dbPlayer == null)
        {
            dbPlayer = await CreateDatabasePlayer(login);
        }

        var player = await Player.Create(_gbxRemoteClient, dbContext, dbPlayer);
        _connectedPlayers.Add(player);

        return player;
    }
}
