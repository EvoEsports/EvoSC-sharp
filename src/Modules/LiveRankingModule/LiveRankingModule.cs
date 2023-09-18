using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;

namespace EvoSC.Modules.Official.LiveRankingModule;

[Module(IsInternal = true)]
public class LiveRankingModule : EvoScModule, IToggleable
{
    private readonly ILiveRankingService _service;

    public LiveRankingModule(ILiveRankingService service)
    {
        _service = service;
    }

    public Task EnableAsync() => _service.OnEnableAsync();

    // if no cleaning for the classes needed to be done, return here a completed task, otherwise clean the classes, and then complete the task. 
    public Task DisableAsync() => _service.OnDisableAsync();
}
