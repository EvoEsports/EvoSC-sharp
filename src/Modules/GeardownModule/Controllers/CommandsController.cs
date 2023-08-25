using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;

namespace EvoSC.Modules.Evo.GeardownModule.Controllers;

[Controller]
public class CommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IGeardownService _geardown;

    public CommandsController(IGeardownService geardown) => _geardown = geardown;

    [ChatCommand("geardown_setup", "Setup the server for a match from geardown.")]
    public Task GeardownSetupAsync(string matchToken) => _geardown.SetupServerAsync(matchToken);
}
