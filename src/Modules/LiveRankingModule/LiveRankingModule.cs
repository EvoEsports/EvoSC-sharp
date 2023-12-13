using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;

namespace EvoSC.Modules.Official.LiveRankingModule;

[Module(IsInternal = true)]
public class LiveRankingModule(ILiveRankingService service) : EvoScModule, IToggleable
{
    public Task EnableAsync() => service.OnEnableAsync();

    // if no cleaning for the classes needed to be done, return here a completed task, otherwise clean the classes, and then complete the task. 
    public Task DisableAsync() => service.OnDisableAsync();
}
