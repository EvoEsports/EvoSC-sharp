using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class ReadyCommandsController(IReadyService readyService, IServerClient server) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("ready", "Set yourself as ready for the match.")]
    [CommandAlias("/r")]
    [CommandAlias("/rdy")]
    public Task SetReadyAsync() => readyService.SetPlayerReadyStatusAsync(Context.Player, true);
    
    [ChatCommand("unready", "Remove yourself as ready for the match.")]
    [CommandAlias("/ur")]
    [CommandAlias("/unrdy")]
    public Task SetUnreadyAsync() => readyService.SetPlayerReadyStatusAsync(Context.Player, false);

    [ChatCommand("readytest", "Test the ready widget.")]
    public async Task ReadyTestAsync()
    {
        await readyService.ResetAsync();
        await readyService.AddPlayersAsync(Context.Player);
        await readyService.EnableAsync();
        /* await playerReady.ResetReadyWidgetAsync(true);
        await playerReady.AddRequiredPlayers(Context.Player);
        await playerReady.SetWidgetEnabled(true);
        await playerReady.SendWidgetAsync(Context.Player); */
    }

    [ChatCommand("readyfakeplayer", "Add a fake player with the ready widget.")]
    public Task ReadyFakePlayerAsync() => server.Remote.ConnectFakePlayerAsync();
}
