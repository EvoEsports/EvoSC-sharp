using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using EvoSC.CLI;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Interfaces;

namespace EvoSC;

[CliCommand("run", "Start the server controller.")]
[CliOption(typeof(int), "Verbosity level of the output.", "--verbosity", "-v")]
public class RunCommand : ICliCommand
{
    public async Task ExecuteAsync(CancellationToken cancelToken, CliCommandContext context)
    {
        await new Application(context.Args).RunAsync();
    }
}
