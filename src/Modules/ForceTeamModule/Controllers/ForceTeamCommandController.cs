using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;
using EvoSC.Modules.Official.ForceTeamModule.Permissions;

namespace EvoSC.Modules.Official.ForceTeamModule.Controllers;

[Controller]
public class ForceTeamCommandController(IForceTeamService forceTeamService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("forceteam", "Force players to teams.", ForceTeamPermissions.ForcePlayerTeam)]
    [CommandAlias("/ft", hide: true)]
    public Task ForceTeamAsync() => forceTeamService.ShowWindowAsync(Context.Player);

    [ChatCommand("autobalance", "Auto balance players into the teams.", ForceTeamPermissions.AutoTeamBalance)]
    public Task AutoBalanceAsync() => forceTeamService.BalanceTeamsAsync();
}
