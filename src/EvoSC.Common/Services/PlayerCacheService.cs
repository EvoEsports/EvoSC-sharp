using EvoSC.Common.Events;
using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using GbxRemoteNet;
using GbxRemoteNet.Events;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class PlayerCacheService : IPlayerCacheService
{
    private readonly IServerClient _server;
    private readonly ILogger<PlayerCacheService> _logger;
    private readonly IPlayerRepository _playerRepository;
    
    
    private readonly Dictionary<string, IOnlinePlayer> _onlinePlayers = new();
    private readonly object _onlinePlayersMutex = new();

    public IEnumerable<IOnlinePlayer> OnlinePlayers
    {
        get
        {
            lock (_onlinePlayersMutex)
            {
                return _onlinePlayers.Values;
            }
        }
    }

    public PlayerCacheService(IEventManager events, IServerClient server, ILogger<PlayerCacheService> logger, IPlayerRepository playerRepository)
    {
        _server = server;
        _logger = logger;
        _playerRepository = playerRepository;
        
        SetupEvents(events);
    }

    private void SetupEvents(IEventManager events)
    {
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerConnect)
            .WithInstance(this)
            .WithInstanceClass<PlayerCacheService>()
            .WithHandlerMethod<PlayerConnectGbxEventArgs>(OnPlayerConnectAsync)
            .WithPriority(EventPriority.High)
        );
        
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerDisconnect)
            .WithInstance(this)
            .WithInstanceClass<PlayerCacheService>()
            .WithHandlerMethod<PlayerDisconnectGbxEventArgs>(OnPlayerDisconnectAsync)
            .WithPriority(EventPriority.High)
        );
        
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerInfoChanged)
            .WithInstance(this)
            .WithInstanceClass<PlayerCacheService>()
            .WithHandlerMethod<PlayerInfoChangedGbxEventArgs>(OnPlayerInfoChangedAsync)
            .WithPriority(EventPriority.High)
        );

        _server.Remote.OnConnected += OnServerConnectedAsync;
    }

    private async Task OnServerConnectedAsync()
    {
        try
        {
            await UpdatePlayerListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update player list cache");
            throw;
        }
    }

    private async Task OnPlayerInfoChangedAsync(object sender, PlayerInfoChangedGbxEventArgs e)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(e.PlayerInfo.Login);
        await ForceUpdatePlayerInternalAsync(accountId);
    }

    private Task OnPlayerDisconnectAsync(object sender, PlayerDisconnectGbxEventArgs e)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(e.Login);

        lock (_onlinePlayersMutex)
        {
            if (_onlinePlayers.ContainsKey(accountId))
            {
                _onlinePlayers.Remove(accountId);
            }
        }
        
        return Task.CompletedTask;
    }

    private async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs e)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(e.Login);
        
        try
        {
            await ForceUpdatePlayerInternalAsync(accountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache player with account ID {AccountId}", accountId);
        }
    }

    private async Task ForceUpdatePlayerInternalAsync(string accountId)
    {
        var player = await GetOnlinePlayerCachedAsync(accountId, true);

        if (player == null)
        {
            throw new InvalidOperationException(
                $"Tried to get online player with account ID '{accountId}', but the player object is null");
        }

        lock (_onlinePlayersMutex)
        {
            _onlinePlayers[accountId] = player;
        }
    }

    public Task<IOnlinePlayer?> GetOnlinePlayerCachedAsync(string accountId) => GetOnlinePlayerCachedAsync(accountId, false);

    public async Task<IOnlinePlayer?> GetOnlinePlayerCachedAsync(string accountId, bool forceUpdate)
    {
        lock (_onlinePlayersMutex)
        {
            if (!forceUpdate && _onlinePlayers.ContainsKey(accountId))
            {
                return _onlinePlayers[accountId];
            }
        }
        
        var playerLogin = PlayerUtils.ConvertAccountIdToLogin(accountId);

        // get all info about the online player
        var result = await _server.Remote.MultiCallAsync(new MultiCall()
            .Add(nameof(GbxRemoteClient.GetPlayerInfoAsync), playerLogin)
            .Add(nameof(GbxRemoteClient.GetDetailedPlayerInfoAsync), playerLogin)
        );

        var onlinePlayerInfo = GbxRemoteUtils.DynamicToType<TmPlayerInfo>(result[0]);
        var onlinePlayerDetails = GbxRemoteUtils.DynamicToType<TmPlayerDetailedInfo>(result[1]);
        
        if (onlinePlayerDetails == null || onlinePlayerInfo == null)
        {
            throw new PlayerNotFoundException(accountId, $"Cannot find online player: {accountId}");
        }

        // get the player from the database or create if they don't exist
        var player = await GetOrCreatePlayerAsync(accountId, onlinePlayerDetails);

        if (player == null)
        {
            throw new PlayerNotFoundException(accountId, "Failed to fetch or create player in the database.");
        }

        return new OnlinePlayer(player)
        {
            State = onlinePlayerDetails.GetState(),
            Flags = onlinePlayerInfo.GetFlags()
        };
    }

    private async Task<IPlayer> GetOrCreatePlayerAsync(string accountId, TmPlayerDetailedInfo onlinePlayerDetails)
    {
        var player = await _playerRepository.GetPlayerByAccountIdAsync(accountId) ??
                     await _playerRepository.AddPlayerAsync(accountId, onlinePlayerDetails);
        return player;
    }

    public async Task UpdatePlayerListAsync()
    {
        var onlinePlayers = await _server.Remote.GetPlayerListAsync();

        var multiCall = new MultiCall();

        foreach (var player in onlinePlayers)
        {
            if (player.IsServer())
            {
                continue;
            }
            
            multiCall.Add(nameof(GbxRemoteClient.GetPlayerInfoAsync), player.Login)
                .Add(nameof(GbxRemoteClient.GetDetailedPlayerInfoAsync), player.Login);
        }

        var callResult = await _server.Remote.MultiCallAsync(multiCall);

        if (callResult.Length < (onlinePlayers.Length-1) * 2)
        {
            throw new InvalidOperationException(
                $"Missing player information. {callResult.Length / 2} results returned but need {onlinePlayers.Length - 1}.");
        }

        for (var i = 0; i < callResult.Length; i += 2)
        {
            var onlinePlayerInfo = GbxRemoteUtils.DynamicToType<TmPlayerInfo>(callResult[i]);
            var onlinePlayerDetails = GbxRemoteUtils.DynamicToType<TmPlayerDetailedInfo>(callResult[i + 1]);

            var accountId = PlayerUtils.ConvertLoginToAccountId(onlinePlayerInfo.Login);
            
            var player = await GetOrCreatePlayerAsync(accountId, onlinePlayerDetails);
            
            if (player == null)
            {
                throw new PlayerNotFoundException(accountId, "Failed to fetch or create player in the database.");
            }
            
            var onlinePlayer = new OnlinePlayer(player)
            {
                State = onlinePlayerDetails.GetState(),
                Flags = onlinePlayerInfo.GetFlags()
            };

            lock (_onlinePlayersMutex)
            {
                _onlinePlayers[accountId] = onlinePlayer;
            }
            
            _logger.LogDebug("Cached online player '{AccountId}'", accountId);
        }
    }

    public Task UpdatePlayerAsync(IPlayer player) => ForceUpdatePlayerInternalAsync(player.AccountId);
}
