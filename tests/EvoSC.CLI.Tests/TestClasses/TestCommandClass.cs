using System.CommandLine.Invocation;
using System.Threading.Tasks;
using EvoSC.CLI.Attributes;

namespace EvoSC.CLI.Tests.TestClasses;

[CliCommand(Name = "Test", Description = "Simple command class.")]
public class TestCommandClass
{
    public Task ExecuteAsync(InvocationContext context, int myarg)
    {
        context.ExitCode = myarg;
        return Task.CompletedTask;
    }
}
