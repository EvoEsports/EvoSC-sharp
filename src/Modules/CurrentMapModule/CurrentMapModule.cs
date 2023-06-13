using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;

namespace EvoSC.Modules.Official.CurrentMapModule;

[Module(IsInternal = true)]
public class CurrentMapModule : EvoScModule, IToggleable
{
    private readonly ICurrentMapService _service;

    public CurrentMapModule(ICurrentMapService service)
    {
        _service = service;
    }

    public Task EnableAsync() => _service.ShowWidgetAsync();
    public Task DisableAsync() => _service.HideWidgetAsync();
}
