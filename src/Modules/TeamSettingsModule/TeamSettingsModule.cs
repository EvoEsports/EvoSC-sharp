using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;

namespace EvoSC.Modules.Official.TeamSettingsModule;

[Module(IsInternal = true)]
public class TeamSettingsModule(ITeamSettingsService teamSettingsService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => Task.CompletedTask;

    public Task DisableAsync() => teamSettingsService.HideTeamSettingsForEveryoneAsync();
}
