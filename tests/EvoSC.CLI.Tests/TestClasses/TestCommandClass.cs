using System.Threading.Tasks;
using EvoSC.CLI.Attributes;
using EvoSC.CLI.Interfaces;

namespace EvoSC.CLI.Tests.TestClasses;

[CliCommand(Name = "Test", Description = "Simple command class.")]
public class TestCommandClass
{
    public Task ExecuteAsync(ICliContext context, int myarg)
    {
        context.ExitCode = myarg;
        return Task.CompletedTask;
    }
}
