using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard;

[Module(IsInternal = true)]
public class ScoreboardModule : EvoScModule, IToggleable
{
    private readonly IScoreboardService _scoreboardService;

    public ScoreboardModule(IScoreboardService scoreboardService)
    {
        _scoreboardService = scoreboardService;
    }

    public Task EnableAsync()
    {
        return _scoreboardService.HideNadeoScoreboard();
    }

    public Task DisableAsync()
    {
        return _scoreboardService.ShowNadeoScoreboard();
    }
}
