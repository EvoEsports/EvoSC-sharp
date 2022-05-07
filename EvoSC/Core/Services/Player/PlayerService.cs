using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Domain;
using EvoSC.Interfaces.Players;
using GbxRemoteNet;
using GbxRemoteNet.XmlRpc.Types;
using GbxRemoteNet.XmlRpc;
using GbxRemoteNet.Structs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EvoSC.Core.Configuration;
using EvoSC.Core.Helpers;
using NLog;

namespace EvoSC.Core.Services.Player;

public class PlayerService : IPlayerService
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly DatabaseContext _databaseContext;
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly IPlayerCallbacks _playerCallbacks;
    private readonly Theme _Theme;

    private readonly List<Domain.Players.Player> _connectedPlayers = new();

    public PlayerService(DatabaseContext databaseContext, GbxRemoteClient gbxRemoteClient, IPlayerCallbacks playerCallbacks, Theme theme)
    {
        _databaseContext = databaseContext;
        _gbxRemoteClient = gbxRemoteClient;
        _playerCallbacks = playerCallbacks;
        _Theme = theme;
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
            var player = await _databaseContext.Players.FirstOrDefaultAsync(dbPlayer => dbPlayer.Login == playerOnline.Login);

            if (player == null)
            {
                var detailedInfo = await _gbxRemoteClient.GetDetailedPlayerInfoAsync(playerOnline.Login);
                player = await CreatePlayer(detailedInfo);
            }

            _connectedPlayers.Add(player);
        }
    }

    public async Task ClientOnPlayerConnect(string login, bool isspectator)
    {
        var firstConnect = false;
        var player = await _databaseContext.Players.FirstOrDefaultAsync(player => player.Login == login);

        if (player == null)
        {
            firstConnect = true;
            var playerInfo = await _gbxRemoteClient.GetDetailedPlayerInfoAsync(login);
            player = await CreatePlayer(playerInfo);
        }

        _connectedPlayers.Add(player);
        _playerCallbacks.OnPlayerConnect(new PlayerConnectEventArgs(player));

        _logger.Info($"Player {player.UbisoftName} ({login}) connected to the server.");

        if (firstConnect)
        {
            var msg = new ChatMessage();
            msg.SetMessage($"Player {ChatMessage.GetHighlightedString(player.UbisoftName)} connected to the server for the first time.");
            msg.SetInfo();
            msg.SetIcon(Icon.Globe);

            await _gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());
        }
        else
        {
            var msg = new ChatMessage();
            msg.SetMessage($"Player {ChatMessage.GetHighlightedString(player.UbisoftName)} connected to the server. Last Visit: {ChatMessage.GetHighlightedString(player.LastVisit.ToString())}");
            msg.SetInfo();
            msg.SetIcon(Icon.Globe);

            await _gbxRemoteClient.ChatSendServerMessageAsync(msg.Render());
        }
        player.LastVisit = DateTime.UtcNow;
        await _databaseContext.SaveChangesAsync();
    }

    public async Task ClientOnPlayerDisconnect(string login, string reason)
    {
        var player = _connectedPlayers.FirstOrDefault(player => player.Login == login);

        if (player == null)
        {
            _logger.Warn($"A disconnecting player was not found in the connected players list. Something went wrong. Player login: {login}");
            return;
        }

        _connectedPlayers.Remove(player);
        player.LastVisit = DateTime.UtcNow;

        _databaseContext.Players.Update(player);
        await _databaseContext.SaveChangesAsync();

        _playerCallbacks.OnPlayerDisconnect(new PlayerDisconnectEventArgs(player, reason));
    }

    public List<Domain.Players.Player> GetConnectedPlayers()
    {
        return _connectedPlayers;
    }

    private async Task<Domain.Players.Player> CreatePlayer(PlayerDetailedInfo playerInfo)
    {
        var player = new Domain.Players.Player
        {
            Login = playerInfo.Login,
            UbisoftName = playerInfo.NickName,
            Group = 0,
            Path = playerInfo.Path,
            Banned = false,
            LastVisit = DateTime.MinValue,
            IsSpectator = playerInfo.IsSpectator
        };

        _databaseContext.Players.Add(player);
        await _databaseContext.SaveChangesAsync();
        return player;
    }
}
