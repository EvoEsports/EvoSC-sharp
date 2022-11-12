using System.CommandLine.Invocation;

namespace EvoSC.CLI;

public class CliCommandContext
{
    public required InvocationContext InvocationContext { get; init;  }
    public required string[] Args { get; init; }
}
