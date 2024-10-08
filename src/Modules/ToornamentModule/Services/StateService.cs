using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class StateService : IStateService
{
    private bool _initialSetup;
    private string _matchSettingsName = "";
    private readonly object _initialSetupLock = new();
    private bool _setupFinished;
    private readonly object _setupFinishedLock = new();
    private bool _waitingForMatchStart;
    private readonly object _waitingForMatchStartLock = new();
    private bool _matchInProgress;
    private readonly object _matchInProgressLock = new();
    private bool _matchEnded;
    private readonly object _matchEndedLock = new();

    public bool IsInitialSetup
    {
        get
        {
            lock (_initialSetupLock)
            {
                return _initialSetup;
            }
        }
    }

    public string MatchSettingsName
    {
        get
        {
            lock (_initialSetupLock)
            {
                return _matchSettingsName;
            }
        }
    }

    public bool SetupFinished
    {
        get
        {
            lock (_setupFinishedLock)
            {
                return _setupFinished;
            }
        }
    }

    public bool MatchInProgress
    {
        get
        {
            lock (_matchInProgressLock)
            {
                return _matchInProgress;
            }
        }
    }

    public bool MatchEnded
    {
        get
        {
            lock (_matchEndedLock)
            {
                return _matchEnded;
            }
        }
    }

    public bool WaitingForMatchStart
    {
        get
        {
            lock (_waitingForMatchStartLock)
            {
                return _waitingForMatchStart;
            }
        }
    }

    public void SetInitialSetup(string matchSettingsName)
    {
        lock (_initialSetupLock)
        {
            _initialSetup = true;
            _matchSettingsName = matchSettingsName;
        }

        lock (_setupFinishedLock)
        {
            _setupFinished = false;
        }

        lock (_waitingForMatchStartLock)
        {
            _waitingForMatchStart = false;
        }

        lock (_matchInProgressLock)
        {
            _matchInProgress = false;
        }

        lock (_matchEndedLock)
        {
            _matchEnded = false;
        }
    }

    public void SetSetupFinished()
    {
        lock (_initialSetupLock)
        {
            _initialSetup = false;
        }

        lock (_setupFinishedLock)
        {
            _setupFinished = true;
        }

        lock (_waitingForMatchStartLock)
        {
            _waitingForMatchStart = true;
        }

        lock (_matchInProgressLock)
        {
            _matchInProgress = false;
        }

        lock (_matchEndedLock)
        {
            _matchEnded = false;
        }
    }

    public void SetMatchStarted()
    {
        lock (_initialSetupLock)
        {
            _initialSetup = false;
        }

        lock (_setupFinishedLock)
        {
            _setupFinished = false;
        }

        lock (_waitingForMatchStartLock)
        {
            _waitingForMatchStart = false;
        }

        lock (_matchInProgressLock)
        {
            _matchInProgress = true;
        }

        lock (_matchEndedLock)
        {
            _matchEnded = false;
        }
    }

    public void SetMatchEnded()
    {
        lock (_initialSetupLock)
        {
            _initialSetup = false;
        }

        lock (_setupFinishedLock)
        {
            _setupFinished = false;
        }

        lock (_waitingForMatchStartLock)
        {
            _waitingForMatchStart = false;
        }

        lock (_matchInProgressLock)
        {
            _matchInProgress = false;
        }

        lock (_matchEndedLock)
        {
            _matchEnded = true;
            _matchSettingsName = "";
        }
    }
}
