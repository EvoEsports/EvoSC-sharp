using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.TeamSettingsModule;

[Module(IsInternal = true)]
public class TeamSettingsModule() : EvoScModule, IToggleable
{
    public Task EnableAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        return Task.CompletedTask;
    }
}
