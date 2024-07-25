using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.TeamSettingsModule;

[Module(IsInternal = true)]
public class TeamSettingsModule : EvoScModule, IToggleable
{
    public Task EnableAsync() => Task.CompletedTask;

    public Task DisableAsync() => Task.CompletedTask;
}
