using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using SpectatorTargetInfo.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule;

[Module(IsInternal = true)]
public class SpectatorTargetInfoModule(ISpectatorTargetInfoService spectatorTargetInfoService) : EvoScModule,
    IToggleable
{
    public Task EnableAsync()
    {
        spectatorTargetInfoService.HideNadeoSpectatorInfoAsync();
        return spectatorTargetInfoService.SendManiaLinkAsync();
    }

    public Task DisableAsync() => spectatorTargetInfoService.ShowNadeoSpectatorInfoAsync();
}
