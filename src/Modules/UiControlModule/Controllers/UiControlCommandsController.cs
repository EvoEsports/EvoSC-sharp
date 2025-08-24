using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.UiControlModule.Interfaces;

namespace EvoSC.Modules.Official.UiControlModule.Controllers;

[Controller]
public class UiControlCommandsController(IUiControlService uiControlService)
    : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("uicontrol", "[Command.UiControl]")]
    public Task ShowMenuAsync() =>
        uiControlService.DisplayMenuAsync(Context.Player);
}
