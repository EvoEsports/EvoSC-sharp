using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule;

[Module(IsInternal = true)]
public class SpectatorTargetInfoModule(ISpectatorTargetInfoService spectatorTargetInfoService) : EvoScModule,
    IToggleable
{
    public async Task EnableAsync()
    {
        await spectatorTargetInfoService.HideNadeoSpectatorInfoAsync();
        // return spectatorTargetInfoService.SendManiaLinkAsync();
    }

    public Task DisableAsync() => spectatorTargetInfoService.ShowNadeoSpectatorInfoAsync();
}
