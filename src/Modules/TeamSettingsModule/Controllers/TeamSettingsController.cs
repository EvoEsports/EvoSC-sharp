using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.TeamSettingsModule.Controllers;

[Controller]
public class TeamSettingsController(ITeamSettingsService teamSettingsService, ILogger<TeamSettingsController> logger)
    : ManialinkController
{
    public async Task SaveTeamSettingsAsync(TeamSettingsModel input)
    {
        if (!IsModelValid)
        {
            //TODO: find out why validation errors are not shown in ManiaLink
            
            await teamSettingsService.ShowTeamSettingsAsync(Context.Player, input);
            return;
        }

        logger.LogInformation("Setting team 1 name to: {name}.", input.Team1Name); //TODO: remove line
        logger.LogInformation("Setting team 2 name to: {name}.", input.Team2Name); //TODO: remove line

        await teamSettingsService.SetTeamSettingsAsync(input);
        await HideTeamSettingsAsync();

        Context.AuditEvent
            .Success()
            .WithEventName("EditTeamSettings")
            .HavingProperties(new { TeamSettings = input });
    }

    public async Task HideTeamSettingsAsync()
        => await HideAsync(Context.Player, "TeamSettings.EditTeamSettings");
}
