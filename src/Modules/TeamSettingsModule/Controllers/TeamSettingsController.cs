using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;

namespace EvoSC.Modules.Official.TeamSettingsModule.Controllers;

[Controller]
public class TeamSettingsController(ITeamSettingsService teamSettingsService, Locale locale) : ManialinkController
{
    public async Task SaveTeamSettingsAsync(TeamSettingsModel input)
    {
        if (!IsModelValid)
        {
            await ShowAsync(Context.Player, "TeamSettings.EditTeamSettings",
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
