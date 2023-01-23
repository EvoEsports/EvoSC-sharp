using EvoSC.Modules;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace SimpleModule;

[Module]
public class SimpleModule : EvoScModule, IToggleable
{
    public Task EnableAsync()
    {
        Console.WriteLine("Hello World!");
        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        return Task.CompletedTask;
    }
}
