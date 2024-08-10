using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;

namespace EvoSC.Modules.Official.LiveRankingModule;

[Module(IsInternal = true)]
public class LiveRankingModule(ILiveRankingService service) : EvoScModule, IToggleable
{
    public Task EnableAsync() => service.DetectModeAndRequestScoreAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
