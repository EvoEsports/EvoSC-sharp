using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class PlayerReadyTrackerService : IPlayerReadyTrackerService
{
    private readonly List<IPlayer> _readyPlayers = new();
    private readonly object _readyPlayersLock = new();

    private readonly List<IPlayer> _requiredPlayers = new();
    private readonly object _requiredPlayersLock = new();

    private readonly IEventManager _events;

    private bool _matchStarted;
    private readonly object _matchStartedLock = new();
    
    public IEnumerable<IPlayer> ReadyPlayers
    {
        get
        {
            lock (_readyPlayersLock)
            {
                return _readyPlayers.ToArray();
            }
        }
    }

    public List<IPlayer> RequiredPlayers
    {
        get
        {
            lock (_requiredPlayersLock)
            {
                return _requiredPlayers;
            }
        }
    }

    public bool MatchStarted
    {
        get
        {
            lock (_matchStartedLock)
            {
                return _matchStarted;
            }
        }
    }

    public PlayerReadyTrackerService(IEventManager events) => _events = events;

    private Task FireEventIfAllReadyAsync()
    {
        if (RequiredPlayers.Count() == ReadyPlayers.Count())
        {
            return _events.RaiseAsync(MatchReadyEvents.AllPlayersReady, new AllPlayersReadyEventArgs
            {
                ReadyPlayers = ReadyPlayers
            });
        }

        return Task.CompletedTask;
    }
    
    public async Task SetIsReadyAsync(IPlayer player, bool isReady)
    {
        lock (_readyPlayersLock)
        {
            if (isReady)
            {
                if (!_readyPlayers.Contains(player))
                {
                    _readyPlayers.Add(player);
                }
            }
            else if (_readyPlayers.Any(p => p.AccountId == player.AccountId))
            {
                _readyPlayers.Remove(player);
            }
        }

        await _events.RaiseAsync(MatchReadyEvents.PlayerReadyChanged, new PlayerReadyEventArgs
        {
            Player = player,
            IsReady = isReady
        });

        await FireEventIfAllReadyAsync();
    }

    public Task AddRequiredPlayerAsync(IPlayer player)
    {
        lock (_requiredPlayersLock)
        {
            _requiredPlayers.Add(player);
        }

        return FireEventIfAllReadyAsync();
    }

    public async Task AddRequiredPlayersAsync(IEnumerable<IPlayer> players)
    {
        foreach (var player in players)
        {
            await AddRequiredPlayerAsync(player);
        }
    }

    public void Reset() => Reset(false);

    public void Reset(bool resetRequired)
    {
        lock (_readyPlayersLock)
        {
            _readyPlayers.Clear();
        }

        if (resetRequired)
        {
            lock (_requiredPlayers)
            {
                _requiredPlayers.Clear();
            }
        }
    }

    public void SetMatchStarted(bool started)
    {
        lock (_matchStartedLock)
        {
            _matchStarted = started;
        }
    }
}
