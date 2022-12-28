using System.Data.Common;
using System.Globalization;
using Castle.Components.DictionaryAdapter;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Common.Util.Algorithms;
using EvoSC.Common.Util.Database;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class PlayerManagerService : IPlayerManagerService
{
    private readonly ILogger<PlayerManagerService> _logger;
    private readonly DbConnection _db;
    private readonly IServerClient _server;

    public PlayerManagerService(ILogger<PlayerManagerService> logger, DbConnection db, IServerClient server)
    {
        _logger = logger;
        _db = db;
        _server = server;
    }
    
    public async Task<IPlayer?> GetPlayerAsync(string accountId)
    {
        if (accountId.Equals(PlayerUtils.NadeoPlayer.AccountId, StringComparison.Ordinal))
        {
            return PlayerUtils.NadeoPlayer;
        }
        
        var results = await _db.SelectByColumnAsync<DbPlayer>("Players", "AccountId", accountId);
        return results?.FirstOrDefault();
    }

    public async Task<IPlayer> GetOrCreatePlayerAsync(string accountId)
    {
        if (accountId.Equals(PlayerUtils.NadeoPlayer.AccountId, StringComparison.Ordinal))
        {
            return PlayerUtils.NadeoPlayer;
        }
        
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
            _logger.LogDebug("Player not on server.");
        }

        var dbPlayer = new DbPlayer
        {
            AccountId = accountId.ToLower(CultureInfo.InvariantCulture),
            NickName = playerInfo?.NickName ?? accountId,
            UbisoftName = playerInfo?.NickName ?? accountId,
            Zone = playerInfo?.Path ?? "World",
            LastVisit = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var id = await _db.InsertAsync(dbPlayer);

        return new Player(dbPlayer) {Id = id};
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

    public Task<IOnlinePlayer> GetOnlinePlayerAsync(IPlayer player)
    {
        if (player.IsNadeoPlaceholder())
        {
            throw new InvalidOperationException("Cannot get Nadeo Placeholder Player as an online player.");
        }
        
        return GetOnlinePlayerAsync(player.AccountId);
    }

    public Task UpdateLastVisitAsync(IPlayer player)
    {
        if (player.IsNadeoPlaceholder())
        {
            throw new InvalidOperationException("Cannot update a Nadeo placeholder player object.");
        }
        
        var sql = "update `Players` set LastVisit=@Lastvisit where `Id`=@Id";
        var values = new {LastVisit = DateTime.UtcNow, Id = player.Id};

        return _db.QueryAsync(sql, values);
    }

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

    public async Task<IEnumerable<IOnlinePlayer>> FindOnlinePlayerAsync(string nickname)
    {
        var players = (await GetOnlinePlayersAsync()).ToArray();
        var distances = new List<dynamic>();

        foreach (var player in players)
        {
            var cleanedName = FormattingUtils.CleanTmFormatting(player.NickName);
            var editDistance = StringEditDistance.GetDistance(nickname, cleanedName);

            // need at least 3 matching characters and ignore completely wrong names
            if (editDistance >= cleanedName.Length - 2)
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
