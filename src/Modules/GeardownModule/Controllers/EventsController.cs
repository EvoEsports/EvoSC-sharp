using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using EvoSC.Modules.Official.MatchTrackerModule.Events;
using EvoSC.Modules.Official.MatchTrackerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Evo.GeardownModule.Controllers;

[Controller]
public class EventsController : EvoScController<IEventControllerContext>
{
    private readonly IGeardownService _geardownService;
    private readonly IGeardownSettings _settings;
    private readonly IServerClient _server;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IGeardownService geardownService, IGeardownSettings settings, IServerClient server, ILogger<EventsController> logger)
    {
        _geardownService = geardownService;
        _settings = settings;
        _server = server;
        _logger = logger;
    }

    [Subscribe(MatchReadyEvents.AllPlayersReady)]
    public async Task OnAllPlayersReadyAsync(object sender, AllPlayersReadyEventArgs args)
    {
        if (!_settings.AutomaticMatchStart)
        {
            return;
        }

        try
        {
            await _geardownService.StartMatchAsync();
        }
        catch (Exception ex)
        {
            await _server.ErrorMessageAsync(
                "An error occured while trying to start the match. Contact match admin immediately.");
            _logger.LogError(ex, "Failed to start match on all players ready.");
        }
    }

    [Subscribe(MatchTrackerEvent.StateTracked)]
    public async Task OnMatchStateTracked(object sender, MatchStateTrackedEventArgs args)
    {
        if (args.State.Status == MatchStatus.Ended)
        {
            try
            {
                await _geardownService.EndMatchAsync(args.Timeline);
            }
            catch (Exception ex)
            {
                await _server.ErrorMessageAsync(
                    "Failed to send match results. Take screenshots and contact a match admin.");
                _logger.LogError(ex, "Failed to send match results.");
            }
        }
    }

    [Subscribe(ModeScriptEvent.WarmUpStart)]
    public async Task OnWarmupStart(object sender, EventArgs args)
    {
        try
        {
            await _geardownService.FinishServerSetupAsync();
        }
        catch (Exception)
        {
            await _server.ErrorMessageAsync("Failed to finish match setup. Contact a match admin immediately.");
            throw;
        }
    }
}
