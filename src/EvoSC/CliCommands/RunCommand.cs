using EvoSC.CLI.Attributes;
using EvoSC.Common.Application;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.CliCommands;

[CliCommand(Name = "run", Description = "Start the server controller.")]
[RequiredFeatures(AppFeature.Config)]
public class RunCommand(IEvoScBaseConfig config)
{
    public async Task ExecuteAsync([Alias(Name = "-s")]int something)
    {
        var app = new Application(config);
        await app.RunAsync();
        app.Dispose();
    }
}
