using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using SpectatorTargetInfo.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule;

[Module(IsInternal = true)]
public class SpectatorTargetInfoModule : EvoScModule, IToggleable
{
    private readonly ISpectatorTargetInfoService _spectatorTargetInfoService;

    public SpectatorTargetInfoModule(ISpectatorTargetInfoService spectatorTargetInfoService) =>
        _spectatorTargetInfoService = spectatorTargetInfoService;

    public Task EnableAsync()
    {
        _spectatorTargetInfoService.HideNadeoSpectatorInfoAsync();
        return _spectatorTargetInfoService.SendManiaLinkAsync();
    }

    public Task DisableAsync() => _spectatorTargetInfoService.ShowNadeoSpectatorInfoAsync();
}
