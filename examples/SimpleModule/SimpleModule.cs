using EvoSC.Modules;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.Player.Interfaces;
using Microsoft.Extensions.Logging;

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
