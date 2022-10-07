using System.CommandLine.Invocation;

namespace EvoSC.CLI;

public class CliCommandContext
{
    public InvocationContext InvocationContext { get; }
    public string[] Args { get; }

    public CliCommandContext(InvocationContext invoContext, string[] args)
    {
        InvocationContext = invoContext;
        Args = args;
    }
}
