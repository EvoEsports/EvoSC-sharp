using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Module(IsInternal = true)]
public class ExampleModule : EvoScModule, IToggleable
{
    public ExampleModule(IManialinkActionManager manialinks)
    {
        manialinks.AddActions(typeof(ExampleManialinkController));
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
