using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchReadyModule.Exceptions;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using LinqToDB;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ReadyTrackerService : IReadyTrackerService
{
    private readonly HashSet<IPlayer> _players = [];
    private readonly HashSet<IPlayer> _readyPlayers = [];
    private bool _allReady;
    private bool _enabled;
    private readonly object _mainLock = new();

    public IEnumerable<IPlayer> ReadyPlayers
    {
        get
        {
            lock (_mainLock)
            {
                return _readyPlayers;
            }
        }
    }

    public IEnumerable<IPlayer> Players
    {
        get
        {
            lock (_mainLock)
            {
                return _players;
            }
        }
    }

    public bool AllReady
    {
        get
        {
            lock (_mainLock)
            {
                return _allReady;
            }
        }
    }

    public bool Enabled
    {
        get
        {
            lock (_mainLock)
            {
                return _enabled;
            }
        }
        set
        {
            lock (_mainLock)
            {
                _enabled = value;
            }
        }
    }

    public Task AddPlayerAsync(IPlayer player)
    {
        lock (_mainLock)
        {
            _players.Add(player);
            UpdateAllReadyLockFree();
        }

        return Task.CompletedTask;
    }

    public async Task RemovePlayerAsync(IPlayer player)
    {
        lock (_mainLock)
        {
            _players.Remove(player);
        }

        await RemoveReadyAsync(player);
    }

    public Task AddReadyAsync(IPlayer player)
    {
        lock (_mainLock)
        {
            if (_players.All(p => p.AccountId != player.AccountId))
            {
                throw new PlayerNotAddedMatchReadyException(player);
            }
            
            _readyPlayers.Add(player);

            UpdateAllReadyLockFree();
        }
        
        return Task.CompletedTask;
    }

    public Task RemoveReadyAsync(IPlayer player)
    {
        lock (_mainLock)
        {
            _readyPlayers.Remove(player);
            UpdateAllReadyLockFree();
        }
        
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        lock (_mainLock)
        {
            _players.Clear();
            _readyPlayers.Clear();
            _allReady = false;
        }

        return Task.CompletedTask;
    }

    public Task EnableAsync()
    {
        lock (_mainLock)
        {
            _enabled = true;
        }
        
        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        lock (_mainLock)
        {
            _enabled = false;
        }

        return Task.CompletedTask;
    }

    private void UpdateAllReadyLockFree()
    {
        _allReady = _players.Count > 0 && _readyPlayers.Count == _players.Count;
    }
}
