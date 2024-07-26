using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Permissions;

namespace EvoSC.Modules.Official.TeamSettingsModule.Controllers;

[Controller]
public class TeamSettingsCommandsController(ITeamSettingsService teamSettingsService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("teams", "[Command.TeamSettings]", TeamSettingsPermissions.EditTeamSettings)]
    public async Task EditTeamSettingsAsync()
    {
        await teamSettingsService.ShowTeamSettingsAsync(Context.Player, await teamSettingsService.GetCurrentTeamSettingsModel());
        Context.AuditEvent.Cancel();
    }
}
