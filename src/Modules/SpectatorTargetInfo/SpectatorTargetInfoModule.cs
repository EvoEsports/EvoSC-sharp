using EvoSC.Modules;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using SpectatorTargetInfo.Interfaces;

namespace SpectatorTargetInfo;

[Module(IsInternal = true)]
public class SpectatorTargetInfoModule : EvoScModule, IToggleable
{
    private readonly ISpectatorTargetInfoService _spectatorTargetInfoService;

    public SpectatorTargetInfoModule(ISpectatorTargetInfoService spectatorTargetInfoService)
    {
        _spectatorTargetInfoService = spectatorTargetInfoService;
    }

    public Task EnableAsync()
    {
        _spectatorTargetInfoService.HideNadeoSpectatorInfo();

        return _spectatorTargetInfoService.SendManiaLink();
    }

    public Task DisableAsync()
    {
        _spectatorTargetInfoService.HideManiaLink();

        return _spectatorTargetInfoService.ShowNadeoSpectatorInfo();
    }
}
