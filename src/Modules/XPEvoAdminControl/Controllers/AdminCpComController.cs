using System.Text.Json;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces.CpCom;
using EvoSC.Modules.Official.XPEvoAdminControl.Models.CpCom;
using EvoSC.Modules.Official.XPEvoAdminControl.Models.Dto;
using EvoSC.Modules.Official.XPEvoAdminControl.Settings;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Controllers;

[Controller]
public class AdminCpComController : EvoScController<IEventControllerContext>
{
    public const string ActionPrefix = "XPEvoAdminAction";

    private readonly IXPEvoAdminSettings _settings;
    private readonly IServerClient _server;
    private readonly ILogger<AdminCpComController> _logger;
    private readonly IGeardownService _geardown;

    public AdminCpComController(IXPEvoAdminSettings settings, IServerClient server,
        ILogger<AdminCpComController> logger, IGeardownService geardown)
    {
        _settings = settings;
        _server = server;
        _logger = logger;
        _geardown = geardown;
    }

    [Subscribe(GbxRemoteEvent.Echo)]
    public async Task OnEcho(object sender, EchoGbxEventArgs args)
    {
        if (!args.PublicParam.StartsWith(ActionPrefix) || args.PublicParam.Length <= ActionPrefix.Length)
        {
            return;
        }

        var action = args.PublicParam.Substring(ActionPrefix.Length + 1);

        if (action == "Response")
        {
            return;
        }
        
        var actionInfo = JsonSerializer.Deserialize<CpAction>(args.InternalParam);

        if (!_settings.AccessToken.Equals(actionInfo.AccessToken, StringComparison.Ordinal))
        {
            await RespondErrorAsync("Authorization failed.", actionInfo.ActionId);
            _logger.LogError("Authorization failed");
            return;
        }

        switch (action)
        {
            case "AssignServer":
                await AssignServerActionAsync(actionInfo);
                break;
        }
    }

    public async Task AssignServerActionAsync(ICpAction action)
    {
        var data = ((JsonElement)action.Data).Deserialize<CpActionAssignServerDto>();

        if (data == null)
        {
            await RespondErrorAsync("Invalid action data.", action.ActionId);
            _logger.LogError("Invalid action data for assign server.");
            return;
        }
        
        if (data.MatchId <= 0)
        {
            await RespondErrorAsync($"Invalid match ID: ${data.MatchId}", action.ActionId);
            _logger.LogError("Invalid match id for assign server: {MatchId}", data.MatchId);
            return;
        }

        try
        {
            await _geardown.SetupServerAsync(data.MatchId);
            await RespondSuccessAsync(action.ActionId);
        }
        catch (Exception ex)
        {
            await RespondErrorAsync($"Failed to setup match (ID: {data.MatchId}): " + ex.Message, action.ActionId);
            _logger.LogError(ex, "Failed to setup server for match ID: {MatchId}", data.MatchId);
        }
    }
    
    private Task RespondAsync(ICpAction action, Guid actionId)
    {
        action.ActionId = actionId;
        var packet = JsonSerializer.Serialize((object)action);
        return _server.Remote.EchoAsync($"XPEvoAdminAction.Response", packet);
    }

    private Task RespondErrorAsync(string message, Guid actionId) =>
        RespondAsync(new CpAction
        {
            AccessToken = null, 
            Action = $"{ActionPrefix}.Response", 
            Data = new
            {
                Success = false,
                ErrorMessage = message
            }
        }, actionId);

    private Task RespondSuccessAsync(Guid actionId) =>
        RespondAsync(new CpAction
        {
            AccessToken = null,
            Action = $"{ActionPrefix}.Response", 
            Data = new { Success = true }
        }, actionId);
}
