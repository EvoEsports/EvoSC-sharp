using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.TeamSettingsModule;

[Module(IsInternal = true)]
public class TeamSettingsModule : EvoScModule, IToggleable
{
    public Task EnableAsync()
    {
        //TODO: implement enable async

        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        //TODO: implement enable async

        return Task.CompletedTask;
    }
}
