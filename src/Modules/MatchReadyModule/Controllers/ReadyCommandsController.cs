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
    private readonly IPlayerReadyTrackerService _readyTracker;

    public ReadyCommandsController(IPlayerReadyService playerReady, IPlayerReadyTrackerService readyTracker)
    {
        _playerReady = playerReady;
        _readyTracker = readyTracker;
    }

    [ChatCommand("ready", "Set yourself as ready for the match.")]
    [CommandAlias("/r")]
    [CommandAlias("/rdy")]
    public Task SetReadyAsync() => _playerReady.SetPlayerReadyStatusAsync(Context.Player, true);
    
    [ChatCommand("unready", "Remove yourself as ready for the match.")]
    [CommandAlias("/ur")]
    public Task SetUnreadyAsync() => _playerReady.SetPlayerReadyStatusAsync(Context.Player, false);

    [ChatCommand("readytest", "yeo")]
    public async Task ReadyTestAsync()
    {
        await _playerReady.ResetReadyWidgetAsync(true);
        await _readyTracker.AddRequiredPlayerAsync(Context.Player);
        await _playerReady.SetWidgetEnabled(true);
        await _playerReady.SendWidgetAsync(Context.Player);
    }
}
