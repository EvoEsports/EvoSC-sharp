using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;

namespace EvoSC.Modules.Evo.GeardownModule.Controllers;

[Controller]
public class GeardownCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IGeardownService _geardown;
    private readonly IServerClient _server;

    public GeardownCommandsController(IGeardownService geardown, IServerClient server)
    {
        _geardown = geardown;
        _server = server;
    }

    [ChatCommand("geardown_setup", "Setup the server for a match from geardown.")]
    public async Task GeardownSetupAsync(int matchId)
    {
        try
        {
            await _server.InfoMessageAsync($"Setting up the match for ID {matchId}, please wait ...", Context.Player);
            await _geardown.SetupServerAsync(matchId);
            await _server.SuccessMessageAsync("Match successfully set up! Reloading match settings ...",
                Context.Player);
        }
        catch (InvalidOperationException ex)
        {
            await _server.ErrorMessageAsync($"(Geardown) {ex.Message}", Context.Player);
        }
        catch (Exception)
        {
            await _server.ErrorMessageAsync($"(Geardown) An unknown error occured, check console.");
        }
    }

    [ChatCommand("geardown_startmatch", "Start a geardown controller match.")]
    public Task GeardownStartMatchAsync() => _geardown.StartMatchAsync();

    [ChatCommand("setpoints", "Set points for a player")]
    public Task SetPlayerPointsAsync(IOnlinePlayer player, int points)
    {
        return _server.Remote.TriggerModeScriptEventArrayAsync("Trackmania.SetPlayerPoints", player.GetLogin(), "", "",
            points.ToString());
    }
}
