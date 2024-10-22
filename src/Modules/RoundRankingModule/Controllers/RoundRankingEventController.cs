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
        var player = await playerManagerService.GetOnlinePlayerAsync(args.AccountId);

        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player,
            CheckpointId = args.CheckpointInLap,
            Time = RaceTime.FromMilliseconds(args.LapTime),
            IsFinish = args.IsEndLap,
            IsDNF = false
        });
    }

    [Subscribe(ModeScriptEvent.GiveUp)]
    public async Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs args)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(args.AccountId);

        await roundRankingService.ConsumeCheckpointDataAsync(new CheckpointData
        {
            Player = player,
            CheckpointId = -1,
            Time = RaceTime.FromMilliseconds(0),
            IsFinish = false,
            IsDNF = true
        });
    }

    [Subscribe(ModeScriptEvent.EndRoundEnd)]
    public Task OnEndRoundAsync(object sender, EventArgs args) =>
        roundRankingService.ClearCheckpointDataAsync();

    [Subscribe(ModeScriptEvent.WarmUpEndRound)]
    public Task OnWarmUpEndRoundAsync(object sender, WarmUpRoundEventArgs args) =>
        roundRankingService.ClearCheckpointDataAsync();

    [Subscribe(ModeScriptEvent.StartMatchStart)]
    public Task OnStartMatchAsync(object sender, WarmUpRoundEventArgs args) =>
        roundRankingService.ClearCheckpointDataAsync();

    [Subscribe(ModeScriptEvent.StartLine)]
    public Task OnStartLineAsync(object sender, PlayerUpdateEventArgs args) =>
        roundRankingService.RemovePlayerCheckpointDataAsync(args.AccountId);

    [Subscribe(ModeScriptEvent.Respawn)]
    public Task OnRespawnAsync(object sender, PlayerUpdateEventArgs args) =>
        roundRankingService.RemovePlayerCheckpointDataAsync(args.AccountId);

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync(object sender, EventArgs args) =>
        roundRankingService.HideRoundRankingWidgetAsync();

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnStartMapAsync(object sender, EventArgs args)
    {
        await roundRankingService.FetchAndCacheTeamInfoAsync();
        await roundRankingService.LoadPointsRepartitionFromSettingsAsync();
        await roundRankingService.ClearCheckpointDataAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpStart)]
    public Task OnWarmUpStartAsync(object sender, EventArgs args) =>
        roundRankingService.SetIsTimeAttackModeAsync(true);

    [Subscribe(ModeScriptEvent.WarmUpEnd)]
    public Task OnWarmUpEndAsync(object sender, EventArgs args) =>
        roundRankingService.SetIsTimeAttackModeAsync(false);
}
