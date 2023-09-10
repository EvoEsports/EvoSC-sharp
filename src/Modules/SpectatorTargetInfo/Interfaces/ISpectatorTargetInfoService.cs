using EvoSC.Common.Interfaces.Models;

namespace SpectatorTargetInfo.Interfaces;

public interface ISpectatorTargetInfoService
{
    public Task SendManiaLink();
    public Task SendManiaLink(string playerLogin);
    public Task HideManiaLink();
    public Task HideNadeoSpectatorInfo();
    public Task ShowNadeoSpectatorInfo();
}
