using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoManialinkController(ISpectatorTargetInfoService spectatorTargetInfoService)
    : ManialinkController
{
    public async Task ReportSpectatorTargetAsync(string targetLogin)
    {
        var spectatorLogin = Context.Player.GetLogin();

        if (targetLogin != "" && targetLogin != spectatorLogin)
        {
            await spectatorTargetInfoService.SetSpectatorTargetAndSendAsync(spectatorLogin, targetLogin);
        }
        else
        {
            await spectatorTargetInfoService.RemovePlayerAsync(spectatorLogin);
            await spectatorTargetInfoService.HideSpectatorInfoWidgetAsync(spectatorLogin);
        }
    }
}
