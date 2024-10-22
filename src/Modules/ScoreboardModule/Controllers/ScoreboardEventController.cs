using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardEventController(
    IScoreboardService scoreboardService,
    IScoreboardNicknamesService nicknamesService
)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnectAsync(object sender, PlayerGbxEventArgs args) =>
        nicknamesService.AddNicknameByLoginAsync(args.Login);

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        await nicknamesService.LoadNicknamesAsync();
        await scoreboardService.SetCurrentRoundAsync(1);
        await scoreboardService.SendScoreboardAsync();
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnStartRoundAsync(object sender, RoundEventArgs args) =>
        scoreboardService.SetCurrentRoundAsync(args.Count);

    [Subscribe(ModeScriptEvent.EndRoundEnd)]
    public Task OnEndRoundAsync(object sender, RoundEventArgs args) =>
        scoreboardService.SetCurrentRoundAsync(args.Count + 1);

    [Subscribe(ModeScriptEvent.WarmUpStart)]
    public async Task OnWarmUpStartAsync(object sender, EventArgs args)
    {
        await scoreboardService.SetIsWarmUpAsync(true);
        await scoreboardService.SetCurrentRoundAsync(1);
        await scoreboardService.SendMetaDataAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpEnd)]
    public async Task OnWarmUpEndAsync(object sender, EventArgs args)
    {
        await scoreboardService.SetIsWarmUpAsync(false);
        await scoreboardService.SetCurrentRoundAsync(1);
        await scoreboardService.SendMetaDataAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpStartRound)]
    public async Task OnWarmUpStartRoundAsync(object sender, WarmUpRoundEventArgs args)
    {
        await scoreboardService.SetIsWarmUpAsync(true);
        await scoreboardService.SetCurrentRoundAsync(args.Current);
    }
}
