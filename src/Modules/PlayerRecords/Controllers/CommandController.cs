using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class CommandController : EvoScController<CommandInteractionContext>
{
    private readonly IPlayerRecordHandlerService _playerRecordHandler;

    public CommandController(IPlayerRecordHandlerService playerRecordHandler) =>
        _playerRecordHandler = playerRecordHandler;

    [ChatCommand("pb", "Show your best time on the current map.")]
    public Task ShowPb() => _playerRecordHandler.ShowCurrentPlayerPbAsync(Context.Player);
}
