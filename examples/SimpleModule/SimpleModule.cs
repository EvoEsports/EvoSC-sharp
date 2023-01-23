using EvoSC.Modules;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace SimpleModule;

[Module]
public class SimpleModule : EvoScModule, IToggleable
{
    public Task Enable()
    {
        Console.WriteLine("Hello World!");
        return Task.CompletedTask;
    }

    public Task Disable()
    {
        return Task.CompletedTask;
    }
}
