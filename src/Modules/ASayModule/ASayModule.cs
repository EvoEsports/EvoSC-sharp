using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ASayModule.Interfaces;

namespace EvoSC.Modules.Official.ASayModule;

[Module(IsInternal = true)]
public class ASayModule : EvoScModule, IToggleable
{
    private readonly IASayService _service;

    public ASayModule(IASayService service)
    {
        _service = service;
    }

    public Task EnableAsync() => Task.CompletedTask;

    // if no cleaning for the classes needed to be done, return here a completed task, otherwise clean the classes, and then complete the task. 
    public Task DisableAsync() => _service.OnDisableAsync();
}
