using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MatchReadyModule.Events;
using EvoSC.Modules.Official.MatchReadyModule.Events.Args;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class ReadyCommandsController(
    IReadyService readyService,
    IServerClient server,
    IPlayerManagerService players,
    IEventManager events)
    : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("ready", "Set yourself as ready for the match.")]
    [CommandAlias("/r")]
    [CommandAlias("/rdy")]
    public Task SetReadyAsync() => readyService.SetPlayerReadyStatusAsync(Context.Player, true);
    
    [ChatCommand("unready", "Remove yourself as ready for the match.")]
    [CommandAlias("/ur")]
    [CommandAlias("/unrdy")]
    public Task SetUnreadyAsync() => readyService.SetPlayerReadyStatusAsync(Context.Player, false);

    [ChatCommand("readyfakeplayer", "Add a fake player with the ready widget.")]
    public Task ReadyFakePlayerAsync() => server.Remote.ConnectFakePlayerAsync();

    [ChatCommand("readysimulateenable", "Dev testing: raise the MatchReady Enabled event with all currently connected players.")]
    public async Task SimulateEnabledEventAsync()
    {
        var onlinePlayers = await players.GetOnlinePlayersAsync();

        await events.RaiseAsync(MatchReadyEvents.Enabled, new EnabledEventArgs
        {
            Players = onlinePlayers
        });
    }

    [ChatCommand("readysimulatedisable", "Dev testing: raise the MatchReady Disabled event.")]
    public Task SimulateDisabledEventAsync() => events.RaiseAsync(MatchReadyEvents.Disabled, EventArgs.Empty);

    [ChatCommand("readyforceall", "Dev testing: force all tracked players to be ready.")]
    public async Task ForceAllReadyAsync()
    {
        foreach (var player in readyService.Players)
        {
            await readyService.SetPlayerReadyStatusAsync(player, true);
        }
    }
}
