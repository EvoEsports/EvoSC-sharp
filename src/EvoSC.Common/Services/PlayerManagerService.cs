using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Common.Util.Algorithms;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class PlayerManagerService : IPlayerManagerService
{
    private readonly ILogger<PlayerManagerService> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IServerClient _server;

    public PlayerManagerService(ILogger<PlayerManagerService> logger, IPlayerRepository playerRepository, IServerClient server)
    {
        _logger = logger;
        _playerRepository = playerRepository;
        _server = server;
    }

    public async Task<IPlayer?> GetPlayerAsync(string accountId) =>
        await _playerRepository.GetPlayerByAccountIdAsync(accountId);

    public async Task<IPlayer> GetOrCreatePlayerAsync(string accountId)
    {
        var player = await GetPlayerAsync(accountId);

        if (player != null)
        {
            return player;
        }

        return await CreatePlayerAsync(accountId);
    }

    public async Task<IPlayer> CreatePlayerAsync(string accountId)
    {
        var playerLogin = PlayerUtils.ConvertAccountIdToLogin(accountId);

        TmPlayerDetailedInfo? playerInfo = null;
        // TODO: Create player with default properties when limited information is available #81 https://github.com/EvoTM/EvoSC-sharp/issues/81
        try
        {
            playerInfo = await _server.Remote.GetDetailedPlayerInfoAsync(playerLogin);
        }
        catch (Exception)
        {
            _logger.LogDebug("Player not on server");
        }

        if (playerInfo == null)
        {
            throw new InvalidOperationException("Player info is null, cannot create player.");
        }

        return await _playerRepository.AddPlayerAsync(accountId, playerInfo);
    }

    public async Task<IOnlinePlayer> GetOnlinePlayerAsync(string accountId)
    {
        var playerLogin = PlayerUtils.ConvertAccountIdToLogin(accountId);
        // TODO: #74 Optimize Player State Fetching (https://github.com/EvoTM/EvoSC-sharp/issues/74)
        var onlinePlayerInfo = await _server.Remote.GetPlayerInfoAsync(playerLogin);
        var onlinePlayerDetails = await _server.Remote.GetDetailedPlayerInfoAsync(playerLogin);

        if (onlinePlayerDetails == null || onlinePlayerInfo == null)
        {
            throw new PlayerNotFoundException(accountId, $"Cannot find online player: {accountId}");
        }

        var player = await GetOrCreatePlayerAsync(accountId);

        if (player == null)
        {
            throw new PlayerNotFoundException(accountId);
        }

        return new OnlinePlayer(player)
        {
            State = onlinePlayerDetails.GetState(),
            Flags = onlinePlayerInfo.GetFlags()
        };
    }

    public Task<IOnlinePlayer> GetOnlinePlayerAsync(IPlayer player) => GetOnlinePlayerAsync(player.AccountId);

    public Task UpdateLastVisitAsync(IPlayer player) => _playerRepository.UpdateLastVisitAsync(player);

    public async Task<IEnumerable<IOnlinePlayer>> GetOnlinePlayersAsync()
    {
        var players = new List<IOnlinePlayer>();
        var onlinePlayers = await _server.Remote.GetPlayerListAsync();

        foreach (var onlinePlayer in onlinePlayers)
        {
            var flags = onlinePlayer.GetFlags();
            
            if (flags.IsServer)
            { 
                // ignore server player as it's not a real player
                continue;
            }
            
            var accountId = PlayerUtils.ConvertLoginToAccountId(onlinePlayer.Login);
            var playerDetails = await _server.Remote.GetDetailedPlayerInfoAsync(onlinePlayer.Login);
            var player = await GetOrCreatePlayerAsync(accountId);
            
            players.Add(new OnlinePlayer(player)
            {
                State = playerDetails.GetState(),
                Flags = flags
            });
        }

        return players;
    }

    private const int MinMatchingCharacters = 2;
    
    public async Task<IEnumerable<IOnlinePlayer>> FindOnlinePlayerAsync(string nickname)
    {
        var players = (await GetOnlinePlayersAsync()).ToArray();
        var distances = new List<dynamic>();

        foreach (var player in players)
        {
            var cleanedName = FormattingUtils.CleanTmFormatting(player.NickName);
            var editDistance = StringEditDistance.GetDistance(nickname, cleanedName);

            // need at least 3 matching characters and ignore completely wrong names
            if (editDistance >= cleanedName.Length - MinMatchingCharacters)
            {
                continue;
            }
            
            distances.Add(new {Player = player, Distance = editDistance});
        }

        return distances
            .OrderBy(e => e.Distance)
            .Select(e => (IOnlinePlayer)e.Player)
            .ToList();
    }
}
