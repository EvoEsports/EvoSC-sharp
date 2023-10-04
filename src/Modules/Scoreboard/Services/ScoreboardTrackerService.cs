using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardTrackerService : IScoreboardTrackerService
{
    private int _roundsPerMap = -1;
    private readonly object _roundsPerMapLock = new();
    
    private int _pointsLimit = -1;
    private readonly object _pointsLimitLock = new();
    
    private int _currentRound = -1;
    private readonly object _currentRoundLock = new();
    
    private int _maxPlayers = -1;
    private readonly object _maxPlayersLock = new();

    public int RoundsPerMap
    {
        get
        {
            lock (_roundsPerMapLock)
            {
                return _roundsPerMap;
            }
        }

        set
        {
            lock (_roundsPerMapLock)
            {
                _roundsPerMap = value;
            }
        }
    }
    
    public int PointsLimit
    {
        get
        {
            lock (_pointsLimitLock)
            {
                return _pointsLimit;
            }
        }

        set
        {
            lock (_pointsLimitLock)
            {
                _pointsLimit = value;
            }
        }
    }
    
    public int CurrentRound
    {
        get
        {
            lock (_currentRoundLock)
            {
                return _currentRound;
            }
        }

        set
        {
            lock (_currentRoundLock)
            {
                _currentRound = value;
            }
        }
    }
    
    public int MaxPlayers
    {
        get
        {
            lock (_maxPlayersLock)
            {
                return _maxPlayers;
            }
        }

        set
        {
            lock (_maxPlayersLock)
            {
                _maxPlayers = value;
            }
        }
    }
}
