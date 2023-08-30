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

    private int _requiredPlayers;
    private readonly object _requiredPlayersLock = new();

    private readonly IEventManager _events;
    
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

    public int RequiredPlayers
    {
        get
        {
            lock (_requiredPlayersLock)
            {
                return _requiredPlayers;
            }
        }
    }

    public PlayerReadyTrackerService(IEventManager events) => _events = events;

    private Task FireEventIfAllReadyAsync()
    {
        if (RequiredPlayers == ReadyPlayers.Count())
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
                _readyPlayers.Add(player);
            }
            else if (_readyPlayers.Contains(player))
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

    public Task SetRequiredPlayersAsync(int count)
    {
        lock (_requiredPlayersLock)
        {
            _requiredPlayers = count;
        }

        return FireEventIfAllReadyAsync();
    }

    public void Reset()
    {
        lock (_readyPlayersLock)
        {
            _readyPlayers.Clear();
        }
    }
}
