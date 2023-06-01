using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleEventController : EvoScController<EventControllerContext>
{
    private readonly ILogger<ExampleEventController> _logger;
    private readonly IManialinkManager _manialinkses;
    private readonly IPlayerManagerService _players;
    
    public ExampleEventController(ILogger<ExampleEventController> logger, IManialinkManager manialinkses, IPlayerManagerService players)
    {
        _logger = logger;
        _manialinkses = manialinkses;
        _players = players;
    }
    
    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWaypoint(object sender, WayPointEventArgs args)
    {
        _logger.LogInformation("Player waypoint, {Player}: {Time}", args.AccountId, args.RaceTime);
        return Task.CompletedTask;
    }

    [Subscribe(GbxRemoteEvent.ManialinkPageAnswer)]
    public async Task PageAnswer(object sender, ManiaLinkPageActionGbxEventArgs args)
    {
        
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task PlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        /* var player = await _players.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));
        await _manialinkses.SendManialinkAsync(player, "SetName.EditName"); */
    }
}
