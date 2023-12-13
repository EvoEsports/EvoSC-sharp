using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class CommandController(IPlayerRecordHandlerService playerRecordHandler) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("pb", "[Command.Pb]")]
    public Task ShowPb() => playerRecordHandler.ShowCurrentPlayerPbAsync(Context.Player);
}
