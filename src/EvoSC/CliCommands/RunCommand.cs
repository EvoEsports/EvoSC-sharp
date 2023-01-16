using EvoSC.CLI;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Interfaces;

namespace EvoSC.CliCommands;

[CliCommand(Name = "run", Description = "Start the server controller.")]
[CliOption(typeof(int), "Verbosity level of the output.", "--verbosity", "-v")]
public class RunCommand : ICliCommand
{
    public async Task ExecuteAsync(CancellationToken cancelToken, CliCommandContext context)
    {
        var app = new Application(context.Args);
        await app.RunAsync();
        app.Dispose();
    }
}
