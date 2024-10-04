using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;

namespace EvoSC.Modules.Official.RoundRankingModule.Controllers;

[Controller]
public class RoundRankingEventController(
    IRoundRankingService roundRankingService,
    IPlayerManagerService playerManagerService
) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWaypointAsync(object sender, WayPointEventArgs args)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(args.AccountId);

        await roundRankingService.AddCheckpointDataAsync(player, args.CheckpointInLap, args.LapTime, args.IsEndLap);
        await roundRankingService.RemoveCheckpointDataAsync(player, args.CheckpointInLap - 1);
        await roundRankingService.DisplayRoundRankingWidgetAsync();
    }

    [Subscribe(ModeScriptEvent.EndRoundEnd)]
    public async Task OnStartRoundAsync()
    {
        await roundRankingService.ClearCheckpointDataAsync();
        await roundRankingService.DisplayRoundRankingWidgetAsync();
    }

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs args)
    {
        //TODO: set DNF

        return Task.CompletedTask;
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync(object sender, PodiumEventArgs args) =>
        roundRankingService.HideRoundRankingWidgetAsync();

    [Subscribe(ModeScriptEvent.EndMapStart)]
    public Task OnEndMapStartAsync(object sender, MapEventArgs args) =>
        roundRankingService.HideRoundRankingWidgetAsync();
}
