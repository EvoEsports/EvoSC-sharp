using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.Scoreboard.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard;

[Module(IsInternal = true)]
public class ScoreboardModule(IScoreboardService scoreboardService) : EvoScModule, IToggleable
{
    public Task EnableAsync()
    {
        scoreboardService.LoadAndSendRequiredAdditionalInfoAsync();
        scoreboardService.HideNadeoScoreboardAsync();
        
        return scoreboardService.ShowScoreboardToAllAsync();
    }

    public Task DisableAsync()
    {
        return scoreboardService.ShowNadeoScoreboardAsync();
    }
}
