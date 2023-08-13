using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.NextMapModule;

[Module(IsInternal = true)]
public class NextMapModule : EvoScModule, IToggleable
{

    public NextMapModule()
    {
    }

    public Task EnableAsync() => Task.CompletedTask;

    public Task DisableAsync() => Task.CompletedTask;
}
