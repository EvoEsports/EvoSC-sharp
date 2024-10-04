using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Models;

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
        if (!roundRankingService.ShouldCollectCheckpointData(args.AccountId))
        {
            return;
        }

        var player = await playerManagerService.GetOnlinePlayerAsync(args.AccountId);

        await roundRankingService.AddCheckpointDataAsync(new CheckpointData
        {
            Player = player,
            CheckpointId = args.CheckpointInLap,
            Time = RaceTime.FromMilliseconds(args.LapTime),
            IsFinish = args.IsEndLap,
            IsDNF = false
        });
        await roundRankingService.DisplayRoundRankingWidgetAsync();
    }

    [Subscribe(ModeScriptEvent.EndRoundEnd)]
    public async Task OnStartRoundAsync()
    {
        await roundRankingService.ClearCheckpointDataAsync();
        await roundRankingService.DisplayRoundRankingWidgetAsync();
    }

    [Subscribe(ModeScriptEvent.GiveUp)]
    public async Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs args)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(args.AccountId);

        await roundRankingService.AddCheckpointDataAsync(new CheckpointData
        {
            Player = player,
            CheckpointId = -1,
            Time = RaceTime.FromMilliseconds(0),
            IsFinish = false,
            IsDNF = true
        });
        await roundRankingService.DisplayRoundRankingWidgetAsync();
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync(object sender, PodiumEventArgs args) =>
        roundRankingService.HideRoundRankingWidgetAsync();

    [Subscribe(ModeScriptEvent.EndMapStart)]
    public Task OnEndMapStartAsync(object sender, MapEventArgs args) =>
        roundRankingService.HideRoundRankingWidgetAsync();

    [Subscribe(ModeScriptEvent.StartMapEnd)]
    public Task OnStartMapAsync(object sender, MapEventArgs args) =>
        roundRankingService.UpdatePointsRepartitionAsync();
}
