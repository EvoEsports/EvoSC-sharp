using EvoSC.Common.Events;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.CoreEvents;
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
    private readonly IEventManager _events;

    private readonly HashSet<string> _newPlayers = new();
    private readonly Dictionary<string, IOnlinePlayer> _onlinePlayers = new();
    private readonly object _onlinePlayersMutex = new();
    private readonly SemaphoreSlim _updatePlayerSem = new(1, 1);

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

    public PlayerCacheService(IEventManager events, IServerClient server, ILogger<PlayerCacheService> logger,
        IPlayerRepository playerRepository)
    {
        _server = server;
        _logger = logger;
        _playerRepository = playerRepository;
        _events = events;

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
    }

    private async Task OnPlayerInfoChangedAsync(object sender, PlayerInfoChangedGbxEventArgs e)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(e.PlayerInfo.Login);
        var player = await ForceUpdatePlayerInternalAsync(accountId);

        if (player.NewPlayer)
        {
            lock (_onlinePlayersMutex)
            {
                _newPlayers.Add(player.Player.AccountId);
            }
        }
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
            var player = await ForceUpdatePlayerInternalAsync(accountId);

            bool isNew;
            lock (_onlinePlayersMutex)
            {
                isNew = player.NewPlayer || _newPlayers.Contains(player.Player.AccountId);

                if (_newPlayers.Contains(player.Player.AccountId))
                {
                    _newPlayers.Remove(player.Player.AccountId);
                }
            }

            await _events.RaiseAsync(PlayerEvents.PlayerJoined,
                new PlayerJoinedEventArgs { Player = player.Player, IsNewPlayer = isNew });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cache player with account ID {AccountId}", accountId);
        }
    }

    private async Task<(IOnlinePlayer Player, bool NewPlayer)> ForceUpdatePlayerInternalAsync(string accountId)
    {
        var player = await GetOnlinePlayerCachedAsync(accountId, true);

        if (player.Player == null)
        {
            throw new InvalidOperationException(
                $"Tried to get online player with account ID '{accountId}', but the player object is null");
        }

        lock (_onlinePlayersMutex)
        {
            _onlinePlayers[accountId] = player.Player;
        }

        return player;
    }

    public Task<(IOnlinePlayer? Player, bool NewPlayer)> GetOnlinePlayerCachedAsync(string accountId) => GetOnlinePlayerCachedAsync(accountId, false);

    public async Task<(IOnlinePlayer? Player, bool NewPlayer)> GetOnlinePlayerCachedAsync(string accountId, bool forceUpdate)
    {
        lock (_onlinePlayersMutex)
        {
            if (!forceUpdate && _onlinePlayers.ContainsKey(accountId))
            {
                return (_onlinePlayers[accountId], false);
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

        IPlayer? player = null;
        bool isNewPlayer = false;
        await _updatePlayerSem.WaitAsync();
        try
        {
            var gottenPlayer = await GetOrCreatePlayerAsync(accountId, onlinePlayerDetails);
            player = gottenPlayer.Player;
            isNewPlayer = gottenPlayer.IsNew;
        }
        finally
        {
            _updatePlayerSem.Release();
        }

        if (player == null)
        {
            throw new PlayerNotFoundException(accountId, "Failed to fetch or create player in the database.");
        }

        var onlinePlayer = new OnlinePlayer(player)
        {
            State = onlinePlayerDetails.GetState(),
            Flags = onlinePlayerInfo.GetFlags(),
            Team = onlinePlayerDetails.TeamId == 0 ? PlayerTeam.Team1 : PlayerTeam.Team2
        };
        
        lock (_onlinePlayersMutex)
        {
            _onlinePlayers[accountId] = onlinePlayer;
        }
        
        if (isNewPlayer)
        {
            await _events.RaiseAsync(PlayerEvents.NewPlayerAdded, new NewPlayerAddedEventArgs { Player = player });
        }
        
        return (onlinePlayer, isNewPlayer);
    }

    private async Task<(IPlayer Player, bool IsNew)> GetOrCreatePlayerAsync(string accountId, TmPlayerDetailedInfo onlinePlayerDetails)
    {
        try
        {
            var player = await _playerRepository.GetPlayerByAccountIdAsync(accountId);

            if (player != null)
            {
                return (player, false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex,
                "Error when trying to get player with Account ID '{AccountID}'. Will attempt creating it instead",
                accountId);
        }

        var newPlayer = await _playerRepository.AddPlayerAsync(accountId, onlinePlayerDetails);
        return (newPlayer, true);
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
            
            if (player.Player == null)
            {
                throw new PlayerNotFoundException(accountId, "Failed to fetch or create player in the database.");
            }

            var onlinePlayer = new OnlinePlayer(player.Player)
            {
                State = onlinePlayerDetails.GetState(),
                Flags = onlinePlayerInfo.GetFlags(),
                Team = onlinePlayerDetails.TeamId == 0 ? PlayerTeam.Team1 : PlayerTeam.Team2
            };

            lock (_onlinePlayersMutex)
            {
                _onlinePlayers[accountId] = onlinePlayer;
            }

            if (player.IsNew)
            {
                await _events.RaiseAsync(PlayerEvents.NewPlayerAdded, new NewPlayerAddedEventArgs { Player = player.Player });
            }
            
            _logger.LogDebug("Cached online player '{AccountId}'", accountId);
        }
    }

    public Task UpdatePlayerAsync(IPlayer player) => ForceUpdatePlayerInternalAsync(player.AccountId);

    public Task InvalidatePlayerStateAsync(IPlayer player)
    {
        lock (_onlinePlayersMutex)
        {
            if (_onlinePlayers.ContainsKey(player.AccountId))
            {
                _onlinePlayers.Remove(player.AccountId);
            }
        }

        return Task.CompletedTask;
    }
}
