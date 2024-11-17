using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;

namespace EvoSC.Modules.Official.RoundRankingModule.Controllers;

[Controller]
public class RoundRankingEventController(IRoundRankingService roundRankingService)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWaypointAsync(object sender, WayPointEventArgs args)
    {
        await roundRankingService.ConsumeCheckpointAsync(
            args.AccountId,
            args.CheckpointInLap,
            args.LapTime,
            args.IsEndLap,
            false
        );
    }

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs args) =>
        roundRankingService.ConsumeDnfAsync(args.AccountId);

    [Subscribe(ModeScriptEvent.EndRoundEnd)]
    public Task OnEndRoundAsync(object sender, EventArgs args) =>
        roundRankingService.ClearCheckpointDataAsync();

    [Subscribe(ModeScriptEvent.WarmUpEndRound)]
    public Task OnWarmUpEndRoundAsync(object sender, WarmUpRoundEventArgs args) =>
        roundRankingService.ClearCheckpointDataAsync();

    [Subscribe(ModeScriptEvent.StartMatchStart)]
    public Task OnStartMatchAsync(object sender, MatchEventArgs args) =>
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
        await roundRankingService.DetectIsTeamsModeAsync();
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
