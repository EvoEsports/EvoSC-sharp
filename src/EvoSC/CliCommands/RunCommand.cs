using EvoSC.CLI.Attributes;
using EvoSC.CLI.Interfaces;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;

namespace EvoSC.CliCommands;

[CliCommand(Name = "run", Description = "Start the server controller.")]
[RequiredFeatures(AppFeature.Config)]
public class RunCommand(IEvoScBaseConfig config, ICliContext cliContext)
{
    public async Task ExecuteAsync()
    {
        var app = new Application(config, cliContext);
        await app.RunAsync();
        app.Dispose();
    }
}
