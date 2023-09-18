using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;

namespace EvoSC.Modules.Official.MatchRankingModule;

[Module(IsInternal = true)]
public class MatchRankingModule : EvoScModule, IToggleable
{
    private readonly IMatchRankingService _matchRankingService;

    public MatchRankingModule(IMatchRankingService matchRankingService)
    {
        _matchRankingService = matchRankingService;
    }
    
    public Task EnableAsync() => _matchRankingService.SendManialink();

    public Task DisableAsync() => _matchRankingService.HideManialink();
}
