using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.RankingModule.Interfaces;

namespace EvoSC.Modules.Official.RankingModule;

[Module(IsInternal = true)]
public class RankingModule(IRankingService rankingService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => rankingService.StartSessionsForOnlinePlayersAsync();

    public Task DisableAsync() => rankingService.EndAllSessionsAsync();
}
