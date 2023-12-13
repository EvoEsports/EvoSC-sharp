using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;

namespace EvoSC.Modules.Official.MatchRankingModule;

[Module(IsInternal = true)]
public class MatchRankingModule(IMatchRankingService matchRankingService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => matchRankingService.SendManialinkToPlayers();

    public Task DisableAsync() => matchRankingService.HideManialink();
}
