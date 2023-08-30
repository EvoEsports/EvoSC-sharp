using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class PlayerReadyTrackerService : IPlayerReadyTrackerService
{
    private readonly List<IPlayer> _readyPlayers = new();
    private readonly object _readyPlayersLock = new();

    private int _requiredPlayers;
    private readonly object _requiredPlayersLock = new();

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

    public void SetIsReady(IPlayer player, bool isReady)
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
    }

    public void SetRequiredPlayers(int count)
    {
        lock (_requiredPlayersLock)
        {
            _requiredPlayers = count;
        }
    }
}
