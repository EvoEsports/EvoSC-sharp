using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorCamModeModule;

[Module(IsInternal = true)]
public class SpectatorCamModeModule(ISpectatorCamModeService spectatorCamModeService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => spectatorCamModeService.InitializeAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
