using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Commands.Manialink;

namespace PluginSample;

public class ExampleManialinkActions : ManialinkCommandGroup
{
    [Command("test", "Just a test")]
    public Task Test()
    {
        Console.WriteLine("manialink answered");
        return Task.CompletedTask;
    }
}