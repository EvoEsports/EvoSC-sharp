using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;

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
        var teamInfos = new TeamSettingsModel { Team1Name = team1Info.Name, Team2Name = team2Info.Name, };

        await teamSettingsService.ShowTeamSettingsAsync(Context.Player, teamInfos);
    }
}
