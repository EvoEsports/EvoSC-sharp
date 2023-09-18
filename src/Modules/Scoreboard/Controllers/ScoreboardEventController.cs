using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.Scoreboard.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardEventController : EvoScController<IEventControllerContext>
{
    private readonly IScoreboardService _scoreboardService;

    public ScoreboardEventController(IScoreboardService scoreboardService)
    {
        _scoreboardService = scoreboardService;
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMap(object sender, MapGbxEventArgs args)
    {
        await _scoreboardService.LoadAndSendRequiredAdditionalInfos();
        await _scoreboardService.ShowScoreboard();
    }

    [Subscribe(MatchSettingsEvent.MatchSettingsLoaded)]
    public async Task OnMatchSettingsLoaded(object sender, MatchSettingsLoadedEventArgs args) =>
        await _scoreboardService.LoadAndSendRequiredAdditionalInfos();

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnRoundStart(object sender, RoundEventArgs args)
    {
        _scoreboardService.SetCurrentRound(args.Count);
        await _scoreboardService.SendRequiredAdditionalInfos();
    }
}
