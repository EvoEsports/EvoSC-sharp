using System.CommandLine.Invocation;
using EvoSC.CLI.Interfaces;

namespace EvoSC.CLI.Models;

public class CliContext(InvocationContext context) : ICliContext
{
    public InvocationContext Context { get; } = context;
}
