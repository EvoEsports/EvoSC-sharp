using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Events;
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

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
        => await _scoreboardService.ShowScoreboard(args.Login);

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public void OnBeginMap() => _scoreboardService.LoadAndUpdateRoundsPerMap();

    [Subscribe(MatchSettingsEvent.MatchSettingsLoaded)]
    public void OnMatchSettingsLoaded() => _scoreboardService.LoadAndUpdateRoundsPerMap();

    [Subscribe(ModeScriptEvent.RoundStart)]
    public void OnRoundStart(object sender, RoundEventArgs args)
    {
        _scoreboardService.SetCurrentRound(args.Round);
        _scoreboardService.SendRoundsInfo();
    }
}
