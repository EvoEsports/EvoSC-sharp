using EvoSC.CLI;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Models;
using EvoSC.Common.Application;
using EvoSC.Common.Config;
using EvoSC.Common.Config.Models;

namespace EvoSC.CliCommands;

[CliCommand(Name = "run", Description = "Start the server controller.")]
[RequiredFeatures(AppFeature.Config)]
public class RunCommand
{
    private readonly IEvoScBaseConfig _config;
    
    public RunCommand(IEvoScBaseConfig config)
    {
        _config = config;
    }
    
    public async Task ExecuteAsync()
    {
        var app = new Application(_config);
        await app.RunAsync();
        app.Dispose();
    }
}
