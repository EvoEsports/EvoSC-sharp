using System.CommandLine.Invocation;

namespace EvoSC.CLI.Interfaces;

public interface ICliCommand
{
    public Task ExecuteAsync(CancellationToken cancelToken, CliCommandContext context);
}
