using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;

namespace EvoSC.Modules.Official.TeamInfoModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class TeamInfoService(IServerClient server, IManialinkManager manialinks) : ITeamInfoService
{
    const string WidgetTemplate = "TeamInfoModule.TeamInfoWidget";

    public async Task SendTeamInfoWidgetAsync(string playerLogin)
    {
        var team1 = await server.Remote.GetTeamInfoAsync(1);
        var team2 = await server.Remote.GetTeamInfoAsync(2);

        await manialinks.SendManialinkAsync(playerLogin, WidgetTemplate, new { team1, team2 });
    }

    public async Task SendTeamInfoWidgetEveryoneAsync()
    {
        var team1 = await server.Remote.GetTeamInfoAsync(1);
        var team2 = await server.Remote.GetTeamInfoAsync(2);

        await manialinks.SendManialinkAsync(WidgetTemplate, new { team1, team2 });
    }

    public async Task HideTeamInfoWidgetAsync(string playerLogin)
    {
        await manialinks.HideManialinkAsync(playerLogin, WidgetTemplate);
    }

    public async Task HideTeamInfoWidgetEveryoneAsync()
    {
        await manialinks.HideManialinkAsync(WidgetTemplate);
    }
}
