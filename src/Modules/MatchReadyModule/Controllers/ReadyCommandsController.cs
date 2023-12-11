using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class ReadyCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IPlayerReadyService _playerReady;
    private readonly IServerClient _server;

    public ReadyCommandsController(IPlayerReadyService playerReady, IServerClient server)
    {
        _playerReady = playerReady;
        _server = server;
    }

    [ChatCommand("ready", "Set yourself as ready for the match.")]
    [CommandAlias("/r")]
    [CommandAlias("/rdy")]
    public Task SetReadyAsync() => _playerReady.SetPlayerReadyStatusAsync(Context.Player, true);
    
    [ChatCommand("unready", "Remove yourself as ready for the match.")]
    [CommandAlias("/ur")]
    public Task SetUnreadyAsync() => _playerReady.SetPlayerReadyStatusAsync(Context.Player, false);

    [ChatCommand("readytest", "Test the ready widget.")]
    public async Task ReadyTestAsync()
    {
        await _playerReady.ResetReadyWidgetAsync(true);
        await _playerReady.AddRequiredPlayers(Context.Player);
        await _playerReady.SetWidgetEnabled(true);
        await _playerReady.SendWidgetAsync(Context.Player);
    }

    [ChatCommand("readyfakeplayer", "Add a fake player with the ready widget.")]
    public async Task ReadyFakePlayerAsync()
    {
        var name = await _server.Remote.ConnectFakePlayerAsync();
        Console.WriteLine(name);
    }
}
