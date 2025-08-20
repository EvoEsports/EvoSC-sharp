using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class MatchReadyEventController(
    IReadyService readyService,
    IPlayerManagerService players,
    IReadyManialinkService readyManialinkService,
    ILogger<MatchReadyEventController> logger)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        if (!readyService.Enabled)
        {
            return;
        }

        var player = await players.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));

        if (player == null)
        {
            logger.LogWarning("Did not obtain player of login {Login}", args.Login);
            return;
        }

        await readyManialinkService.SendWidgetAsync(player);
    }

    [Subscribe(MatchReadyEvents.Enabled)]
    public Task OnMatchReadyEnabled(object sender, EventArgs args) => readyManialinkService.SendWidgetAsync();
    
    [Subscribe(MatchReadyEvents.Disabled)]
    public Task OnMatchReadyDisabled(object sender, EventArgs args) => readyManialinkService.SendWidgetAsync();
}
