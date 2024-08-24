using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.SpectatorTargetInfoModule.Interfaces;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Controllers;

[Controller]
public class SpectatorTargetInfoCommandController(ISpectatorTargetInfoService spectatorTargetInfoService): EvoScController<ICommandInteractionContext>
{
    [ChatCommand("fakeplayer", "Creates fakeplayer.")]
    public Task ShowPb() => spectatorTargetInfoService.AddFakePlayerAsync();
}
