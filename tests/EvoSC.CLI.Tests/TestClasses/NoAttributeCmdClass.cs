using System.Threading.Tasks;

namespace EvoSC.CLI.Tests.TestClasses;

public class NoAttributeCmdClass
{
    public Task ExecuteAsync(int test)
    {
        return Task.CompletedTask;
    }
}
