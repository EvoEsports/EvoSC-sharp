using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Controllers;

[Controller]
public class ReadyCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IPlayerReadyService _playerReady;

    public ReadyCommandsController(IPlayerReadyService playerReady) => _playerReady = playerReady;

    [ChatCommand("ready", "Set yourself as ready for the match.")]
    [CommandAlias("/r")]
    [CommandAlias("/rdy")]
    public Task SetReadyAsync() => _playerReady.SetPlayerReadyStatusAsync(Context.Player, true);
    
    [ChatCommand("unready", "Remove yourself as ready for the match.")]
    [CommandAlias("/ur")]
    public Task SetUnreadyAsync() => _playerReady.SetPlayerReadyStatusAsync(Context.Player, false);
}
