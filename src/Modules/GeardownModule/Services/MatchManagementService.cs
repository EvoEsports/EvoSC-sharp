using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchManagementService : IMatchManagementService
{
    private readonly IServerClient _server;
    
    public MatchManagementService(IServerClient server)
    {
        _server = server;
    }
    
    public Task SetPlayerPointsAsync(IPlayer player, int points)
    {
        return _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.SetPlayerPoints", player.GetLogin(), "", "",
            points.ToString());
    }

    public Task PauseMatchAsync()
    {
        return _server.Remote.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "true");
    }

    public Task UnpauseMatchAsync()
    {
        return _server.Remote.TriggerModeScriptEventArrayAsync("Maniaplanet.Pause.SetActive", "false");
    }
}
