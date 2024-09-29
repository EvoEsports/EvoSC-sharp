using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule;

[Module(IsInternal = true)]
public class ScoreboardModule(IScoreboardService scoreboardService, IScoreboardNicknamesService nicknamesService) : EvoScModule, IToggleable
{
    public Task EnableAsync()
    {
        nicknamesService.LoadNicknamesAsync();
        scoreboardService.HideNadeoScoreboardAsync();
        
        return scoreboardService.SendScoreboardAsync();
    }

    public Task DisableAsync()
    {
        return scoreboardService.ShowNadeoScoreboardAsync();
    }
}
