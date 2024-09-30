using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorCamModeModule.Controllers;

[Controller]
public class SpectatorCamModeEventController(ISpectatorCamModeService camModeService)
    : EvoScController<EventControllerContext>
{
    [Subscribe(ModeScriptEvent.EndRoundStart)]
    public Task OnEndRoundStartAsync(object sender, RoundEventArgs eventArgs) =>
        camModeService.HideCamModeWidgetAsync();
    
    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnStartRoundStartAsync(object sender, RoundEventArgs eventArgs) =>
        camModeService.SendPersistentCamModeWidgetAsync();
}
