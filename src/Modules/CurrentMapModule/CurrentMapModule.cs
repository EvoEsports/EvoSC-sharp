using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;

namespace EvoSC.Modules.Official.CurrentMapModule;

[Module(IsInternal = true)]
public class CurrentMapModule(ICurrentMapService service) : EvoScModule, IToggleable
{
    public Task EnableAsync() => service.ShowWidgetAsync();
    public Task DisableAsync() => service.HideWidgetAsync();
}
