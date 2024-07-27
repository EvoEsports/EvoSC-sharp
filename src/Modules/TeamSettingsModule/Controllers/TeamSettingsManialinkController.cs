using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Permissions;

namespace EvoSC.Modules.Official.TeamSettingsModule.Controllers;

[Controller]
public class TeamSettingsManialinkController(ITeamSettingsService teamSettingsService, Locale locale) : ManialinkController
{
    [ManialinkRoute(Permission = TeamSettingsPermissions.EditTeamSettings)]
    public async Task SaveTeamSettingsAsync(TeamSettingsModel input)
    {
        if (!IsModelValid)
        {
            await ShowAsync(Context.Player, "TeamSettingsModule.EditTeamSettings",
                new { Settings = input, Locale = locale }
            );
            return;
        }

        await teamSettingsService.SetTeamSettingsAsync(input);
        await teamSettingsService.HideTeamSettingsAsync(Context.Player);

        Context.AuditEvent
            .Success()
            .WithEventName("EditTeamSettings")
            .HavingProperties(new { TeamSettings = input });
    }
}
