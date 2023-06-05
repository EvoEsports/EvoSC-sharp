using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.EvoSCTemplateModule.Services;

namespace EvoSC.Modules.Official.EvoSCTemplateModule;


[Module(IsInternal = true)]
public class EvoSCTemplate : EvoScModule, IToggleable
{   
    private readonly EvoSCTemplateService _service;
    
    public EvoSCTemplate(EvoSCTemplateService service)
    {
        _service = service;
    }
    
    public Task EnableAsync() => _service.onEnable();
    
    // if no cleaning for the classes needed to be done, return here a completed task, otherwise clean the classes, and then complete the task. 
    public Task DisableAsync() => _service.onDisable();
}
