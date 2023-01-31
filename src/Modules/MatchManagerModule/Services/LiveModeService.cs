using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Exceptions;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class LiveModeService : ILiveModeService
{
    private Dictionary<string, string> _availableModes = new()
    {
        {"ta", "Trackmania/TM_TimeAttack_Online.Script.txt"}, 
        {"rounds", "Trackmania/TM_Rounds_Online.Script.txt"},
        {"teams", "Trackmania/TM_Teams_Online.Script.txt"},
        {"cup", "Trackmania/TM_Cup_Online.Script.txt"},
        {"laps", "Trackmania/TM_Laps_Online.Script.txt"},
        {"champion", "Trackmania/TM_Champion_Online.Script.txt"},
        {"knockout", "Trackmania/TM_Knockout_Online.Script.txt"}
    };

    private readonly IServerClient _server;
    private readonly IMatchSettingsService _matchSettings;

    public LiveModeService(IServerClient server, IMatchSettingsService matchSettings)
    {
        _server = server;
        _matchSettings = matchSettings;
    }
    
    public IEnumerable<string> GetAvailableModes() => _availableModes.Keys;

    public async Task LoadModeAsync(string mode)
    {
        if (!_availableModes.ContainsKey(mode))
        {
            throw new LiveModeNotFoundException(mode);
        }

        await _server.Remote.SetScriptNameAsync(_availableModes[mode]);
        await _server.Remote.RestartMapAsync();
    }
}
