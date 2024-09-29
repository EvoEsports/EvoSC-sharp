using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardCommandsController(IServerClient server) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("fp", "Spawns fake players.")]
    public async Task SpawnFakePlayerAsync()
    {
        await server.Remote.ConnectFakePlayerAsync();
    }
}
