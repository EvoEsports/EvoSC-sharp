using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Services;

namespace EvoSC.Modules.Official.TeamSettingsModule.Controllers;

[Controller]
public class TeamSettingsCommandsController(IServerClient server, ITeamSettingsService teamSettingsService)
    : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("teams", "[Command.TeamSettings]")]
    public async Task EditTeamSettingsAsync()
    {
        var team1Info = await server.Remote.GetTeamInfoAsync(1);
        var team2Info = await server.Remote.GetTeamInfoAsync(2);
        var info1 = await teamSettingsService.ParseClubLinkUrl(team1Info.ClubLinkUrl);
        var info2 = await teamSettingsService.ParseClubLinkUrl(team2Info.ClubLinkUrl);

        var teamInfos = new TeamSettingsModel
        {
            Team1Name = info1.Get("name") ?? TeamSettingsService.DefaultTeam1Name,
            Team1PrimaryColor = info1.Get("primary") ?? TeamSettingsService.DefaultTeam1Color,
            Team1SecondaryColor = info1.Get("secondary"),
            Team1EmblemUrl = info1.Get("emblem"),
            Team2Name = info2.Get("name") ?? TeamSettingsService.DefaultTeam2Name,
            Team2PrimaryColor = info2.Get("primary") ?? TeamSettingsService.DefaultTeam2Color,
            Team2SecondaryColor = info2.Get("secondary"),
            Team2EmblemUrl = info2.Get("emblem"),
        };

        await teamSettingsService.ShowTeamSettingsAsync(Context.Player, teamInfos);
    }
}
