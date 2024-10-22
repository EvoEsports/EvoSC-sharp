using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule;

[Module(IsInternal = true)]
public class ScoreboardModule(IScoreboardService scoreboardService, IScoreboardNicknamesService nicknamesService)
    : EvoScModule, IToggleable
{
    public async Task EnableAsync()
    {
        await nicknamesService.LoadNicknamesAsync();
        await scoreboardService.HideNadeoScoreboardAsync();
        await scoreboardService.SendScoreboardAsync();
    }

    public Task DisableAsync() =>
        scoreboardService.ShowNadeoScoreboardAsync();
}
