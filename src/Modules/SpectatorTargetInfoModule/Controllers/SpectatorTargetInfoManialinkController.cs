using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoManialinkController(ISpectatorTargetInfoService spectatorTargetInfoService)
    : ManialinkController
{
    public async Task SetSpectatorTargetAsync(string targetLogin)
    {
        if (Context.Player.GetLogin() != targetLogin)
        {
            await spectatorTargetInfoService.SetSpectatorTargetLoginAsync(Context.Player.GetLogin(), targetLogin);
        }
        else
        {
            await spectatorTargetInfoService.RemovePlayerFromSpectatorsListAsync(Context.Player.GetLogin());
            await spectatorTargetInfoService.HideSpectatorInfoWidgetAsync(Context.Player.GetLogin());
        }
    }
}
