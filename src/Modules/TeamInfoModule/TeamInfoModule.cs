using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;

namespace EvoSC.Modules.Official.TeamInfoModule;

[Module(IsInternal = true)]
public class TeamInfoModule(ITeamInfoService teamInfoService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => teamInfoService.InitializeModuleAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
