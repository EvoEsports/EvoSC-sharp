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
using Microsoft.EntityFrameworkCore;
using NLog;

namespace EvoSC.Core.Services.Players;

public class PlayerService : IPlayerService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private static DatabaseContext s_databaseContext;
    private static GbxRemoteClient s_gbxRemoteClient;
    private readonly IPlayerCallbacks _playerCallbacks;

    private static readonly List<Player> s_connectedPlayers = new();

    public PlayerService(DatabaseContext databaseContext, GbxRemoteClient gbxRemoteClient,
        IPlayerCallbacks playerCallbacks)
    {
        s_databaseContext = databaseContext;
        s_gbxRemoteClient = gbxRemoteClient;
        _playerCallbacks = playerCallbacks;
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
            var player =
                await s_databaseContext.Players.FirstOrDefaultAsync(dbPlayer => dbPlayer.Login == playerOnline.Login) ??
                await CreatePlayer(playerOnline.Login);

            s_connectedPlayers.Add(player);
        }
    }

    public async Task ClientOnPlayerConnect(string login, bool isSpectator)
    {
        var firstConnect = false;
        var player = await s_databaseContext.Players.FirstOrDefaultAsync(player => player.Login == login);

        if (player == null)
        {
            firstConnect = true;
            player = await CreatePlayer(login);
        }

        s_connectedPlayers.Add(player);
        _playerCallbacks.OnPlayerConnect(new PlayerConnectEventArgs(player));

        _logger.Info($"Player {player.UbisoftName} ({login}) connected to the server.");
        var msg = new ChatMessage();
        msg.SetInfo();
        msg.SetIcon(Icon.Globe);
        msg.SetMessage(
            $"Player {ChatMessage.GetHighlightedString(player.UbisoftName)} joined the server. Last Visit: {ChatMessage.GetHighlightedString(Snippets.GetRelativeTimeToNow(player.LastVisit))}");
        if (firstConnect)
        {
            msg.SetMessage(
                $"Player {ChatMessage.GetHighlightedString(player.UbisoftName)} joined the server for the first time.");
        }

        await s_gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());
        player.LastVisit = DateTime.UtcNow;
        await s_databaseContext.SaveChangesAsync();
    }

    public async Task ClientOnPlayerDisconnect(string login, string reason)
    {
        _logger.Info("PLAYERDISCONNECT FIRED");
        var player = s_connectedPlayers.FirstOrDefault(player => player.Login == login);

        if (player == null)
        {
            _logger.Warn(
                $"A disconnecting player was not found in the connected players list. Something went wrong. Player login: {login}");
            return;
        }

        var msg = new ChatMessage();
        msg.SetMessage(
            $"Player {ChatMessage.GetHighlightedString(player.UbisoftName)} left the server. Time played: {ChatMessage.GetHighlightedString(Snippets.GetTimeSpentUntilNow(player.LastVisit))}");
        msg.SetInfo();
        msg.SetIcon(Icon.UserRemove);

        await s_gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());

        s_connectedPlayers.Remove(player);
        player.LastVisit = DateTime.UtcNow;

        s_databaseContext.Players.Update(player);
        await s_databaseContext.SaveChangesAsync();

        _playerCallbacks.OnPlayerDisconnect(new PlayerDisconnectEventArgs(player, reason));
    }

    public List<Player> GetConnectedPlayers()
    {
        return s_connectedPlayers;
    }

    private static async Task<Player> CreatePlayer(string login)
    {
        var playerInfo = await s_gbxRemoteClient.GetDetailedPlayerInfoAsync(login);
        var player = new Player
        {
            Login = playerInfo.Login,
            UbisoftName = playerInfo.NickName,
            Group = 0,
            Path = playerInfo.Path,
            Banned = false,
            LastVisit = DateTime.MinValue,
            IsSpectator = playerInfo.IsSpectator
        };

        s_databaseContext.Players.Add(player);
        await s_databaseContext.SaveChangesAsync();
        return player;
    }

    public static async Task<Player> GetPlayer(string login)
    {
        foreach (var player in s_connectedPlayers.Where(player => player.Login == login))
        {
            return player;
        }

        var newPlayer = await CreatePlayer(login);
        s_connectedPlayers.Add(newPlayer);
        return newPlayer;
    }
}
