using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.RoundRankingModule.Controllers;

[Controller]
public class RoundRankingEventController(
    IRoundRankingService roundRankingService,
    IPlayerManagerService playerManagerService,
    ILogger<RoundRankingEventController> logger
) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWaypointAsync(object sender, WayPointEventArgs args)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(args.AccountId);

        logger.LogInformation("Adding data {account} -> {cpId}: {time}", player.AccountId, args.CheckpointInLap,
            args.LapTime);

        await roundRankingService.AddCheckpointDataAsync(player, args.CheckpointInLap, args.LapTime, args.IsEndLap);
        await roundRankingService.DisplayRoundRankingWidgetAsync();
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnStartRoundAsync()
    {
        return Task.CompletedTask;
    }
}
