using System.Text.Json;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Official.XPEvoAdminControl.Events;
using EvoSC.Modules.Official.XPEvoAdminControl.Events.EventArgs;
using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces;
using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces.CpCom;
using EvoSC.Modules.Official.XPEvoAdminControl.Models.Dto;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Controllers;

[Controller]
public class AdminCpComController : EvoScController<IEventControllerContext>
{
    private readonly IServerClient _server;
    private readonly ILogger<AdminCpComController> _logger;
    private readonly IGeardownService _geardown;
    private readonly ICpComService _cpCom;

    public AdminCpComController(IServerClient server, ILogger<AdminCpComController> logger, IGeardownService geardown,
        ICpComService cpCom)
    {
        _server = server;
        _logger = logger;
        _geardown = geardown;
        _cpCom = cpCom;
    }

    [Subscribe(AdminCpEvents.Action)]
    public Task OnAction(object sender, CpComActionEventArgs args)
    {
        switch (args.Action.Action)
        {
            case "AssignServer":
                return AssignServerActionAsync(args.Action);
            case "RestartMatch":
                return RestartMatchAsync(args.Action);
            case "StartMatch":
                return StartMatchAsync(args.Action);
            default:
                return Task.CompletedTask;
        }
    }

    private async Task StartMatchAsync(ICpAction action)
    {
        try
        {
            await _geardown.StartMatchAsync();
            await _cpCom.RespondSuccessAsync(action.ActionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start match.");
            await _cpCom.RespondErrorAsync(ex.Message, action.ActionId);
        }
    }

    private async Task RestartMatchAsync(ICpAction action)
    {
        try
        {
            await _server.Remote.RestartMapAsync();
            await _cpCom.RespondSuccessAsync(action.ActionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restart match.");
            await _cpCom.RespondErrorAsync(ex.Message, action.ActionId);
        }
    }

    private async Task AssignServerActionAsync(ICpAction action)
    {
        var data = ((JsonElement)action.Data).Deserialize<CpActionAssignServerDto>();

        if (data == null)
        {
            await _cpCom.RespondErrorAsync("Invalid action data.", action.ActionId);
            _logger.LogError("Invalid action data for assign server.");
            return;
        }
        
        if (data.MatchId <= 0)
        {
            await _cpCom.RespondErrorAsync($"Invalid match ID: ${data.MatchId}", action.ActionId);
            _logger.LogError("Invalid match id for assign server: {MatchId}", data.MatchId);
            return;
        }

        try
        {
            await _geardown.SetupServerAsync(data.MatchId);
            await _cpCom.RespondSuccessAsync(action.ActionId);
        }
        catch (Exception ex)
        {
            await _cpCom.RespondErrorAsync($"Failed to setup match (ID: {data.MatchId}): " + ex.Message, action.ActionId);
            _logger.LogError(ex, "Failed to setup server for match ID: {MatchId}", data.MatchId);
        }
    }
}
