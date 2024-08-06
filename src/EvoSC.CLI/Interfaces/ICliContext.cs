using System.CommandLine.Invocation;

namespace EvoSC.CLI.Interfaces;

public interface ICliContext
{
    public InvocationContext Context { get; }
}
