using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Services;

namespace EvoSC.Modules.Official.CurrentMapModule;

[Module(IsInternal = true)]
public class CurrentMapModule : EvoScModule, IToggleable
{
    private readonly CurrentMapService _service;

    public CurrentMapModule(CurrentMapService service)
    {
        _service = service;
    }

    public Task EnableAsync() => _service.ShowWidgetAsync();
    public Task DisableAsync() => _service.HideWidgetAsync();
}
