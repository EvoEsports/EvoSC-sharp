using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;

namespace EvoSC.Modules.Official.RoundRankingModule;

[Module(IsInternal = true)]
public class RoundRankingModule(IRoundRankingService roundRankingService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => roundRankingService.DisplayRoundRankingWidgetAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
