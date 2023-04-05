using System.Reflection;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Models;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Module(IsInternal = true)]
public class ExampleModule : EvoScModule, IToggleable
{
    public ExampleModule()
    {
    }
    
    public Task EnableAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        return Task.CompletedTask;
    }
}
