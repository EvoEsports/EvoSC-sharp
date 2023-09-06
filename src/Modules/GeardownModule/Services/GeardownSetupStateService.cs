using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class GeardownSetupStateService : IGeardownSetupStateService
{
    private bool _initialSetup;
    private string _matchSettingsName = "";
    private readonly object _initialSetupLock = new();

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

    public void SetInitialSetup(string matchSettingsName)
    {
        lock (_initialSetupLock)
        {
            _initialSetup = true;
            _matchSettingsName = matchSettingsName;
        }
    }

    public void SetSetupFinished()
    {
        lock (_initialSetupLock)
        {
            _initialSetup = false;
        }
    }
}
