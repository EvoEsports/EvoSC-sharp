using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Services;

namespace EvoSC.Modules.Official.OpenPlanetControl;

[Module(IsInternal = true)]
public class OpenPlanetControl : EvoScModule, IToggleable
{
    private readonly OpenPlanetControlService _service;

    public OpenPlanetControl(OpenPlanetControlService service)
    {
        _service = service;
    }

    public Task EnableAsync() => _service.OnEnableAsync();
    
    public Task DisableAsync() => _service.OnEnableAsync();
}
