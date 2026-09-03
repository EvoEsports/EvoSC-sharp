using System.Drawing;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class MatchReadyEventController(
    IReadyService readyService,
    IPlayerManagerService players,
    IReadyManialinkService readyManialinkService,
    ILogger<MatchReadyEventController> logger,
    IServerClient serverClient)
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
    public async Task OnMatchReadyEnabled(object sender, EnabledEventArgs args)
    {
        readyService.Enabled = true;
        await readyService.ResetAsync();

        await readyService.AddPlayersAsync(args.Players.ToArray());
        
        await readyManialinkService.SendWidgetAsync();
    }

    [Subscribe(MatchReadyEvents.Disabled)]
    public async Task OnMatchReadyDisabled(object sender, EventArgs args)
    {
        readyService.Enabled = false;
        await readyService.ResetAsync();
        
        await readyManialinkService.HideWidgetAsync();
    }

    [Subscribe(MatchReadyEvents.PlayerReadyChanged)]
    public async Task OnReadyChangedAsync(object sender, PlayerReadyEventArgs args)
    {
        await serverClient.Chat.InfoMessageAsync(t => t.AddText(args.Player.NickName)
            .AddText(
                args.IsReady ? " is now ready" : " is no longer ready",
                s => s.WithColor(args.IsReady ? Color.Green : Color.Red)
            )
            .AddText(".")
        );
        
        await readyManialinkService.UpdateWidgetAsync();
    }

    [Subscribe(GbxRemoteEvent.BeginMatch)]
    public Task OnMatchStartedAsync(object sender, EventArgs args) => readyManialinkService.HideWidgetAsync();

    [Subscribe(MatchReadyEvents.AllPlayersReady)]
    public async Task OnAllPlayersReadyAsync(object sender, EventArgs args)
    {
        readyService.Enabled = false;
        await readyManialinkService.HideWidgetAsync();
    }
}
