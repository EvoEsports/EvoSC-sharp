using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Common.Util.Algorithms;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class PlayerManagerService(IPlayerRepository playerRepository, IPlayerCacheService playerCache,
        IServerClient server, ILogger<PlayerManagerService> logger)
    : IPlayerManagerService
{
    public async Task<IPlayer?> GetPlayerAsync(string accountId) =>
        await playerRepository.GetPlayerByAccountIdAsync(accountId);

    public async Task<IPlayer> GetOrCreatePlayerAsync(string accountId)
    {
        try
        {
            var player = await GetPlayerAsync(accountId);

            if (player != null)
            {
                return player;
            }
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex,
                "Error when trying to get player with Account ID '{AccountID}'. Will attempt creating it instead",
                accountId);
        }

        return await CreatePlayerAsync(accountId);
    }

    public Task<IPlayer> CreatePlayerAsync(string accountId) => CreatePlayerAsync(accountId, null);

    public async Task<IPlayer> CreatePlayerAsync(string accountId, string? name)
    {
        var playerLogin = PlayerUtils.ConvertAccountIdToLogin(accountId);

        TmPlayerDetailedInfo? playerInfo = null;
        try
        {
            playerInfo = await server.Remote.GetDetailedPlayerInfoAsync(playerLogin);
        }
        catch (Exception ex)
        {
            logger.LogTrace(ex, "Failed to obtain player info, are they on the server?");
        }

        playerInfo ??= new TmPlayerDetailedInfo {Login = playerLogin, NickName = name ?? accountId};

        return await playerRepository.AddPlayerAsync(accountId, playerInfo);
    }

    public async Task<IOnlinePlayer> GetOnlinePlayerAsync(string accountId)
    {
        var player = await playerCache.GetOnlinePlayerCachedAsync(accountId);

        if (player.Player == null)
        {
            throw new InvalidOperationException(
                $"Failed to get online player with account ID '{accountId}' from cache. Player object is null.");
        }

        return player.Player;
    }

    public Task<IOnlinePlayer> GetOnlinePlayerAsync(IPlayer player) => GetOnlinePlayerAsync(player.AccountId);

    public Task UpdateLastVisitAsync(IPlayer player) => playerRepository.UpdateLastVisitAsync(player);

    public Task<IEnumerable<IOnlinePlayer>> GetOnlinePlayersAsync() => Task.FromResult(playerCache.OnlinePlayers);

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
