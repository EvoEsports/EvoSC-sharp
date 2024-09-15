using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorCamModeModule;

[Module(IsInternal = true)]
public class SpectatorCamModeModule(ISpectatorCamModeService spectatorCamModeService) : EvoScModule, IToggleable
{
    public async Task EnableAsync()
    {
        await spectatorCamModeService.SendPersistentCamModeWidgetAsync();
        await spectatorCamModeService.HideGameModeUiAsync();
    }

    public Task DisableAsync() => Task.CompletedTask;
}
