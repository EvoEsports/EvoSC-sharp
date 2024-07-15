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

    public async Task<dynamic> GetManialinkData()
    {
        var team1 = await server.Remote.GetTeamInfoAsync(1);
        var team2 = await server.Remote.GetTeamInfoAsync(2);
        var roundNumber = 1;
        var infoBoxText = "FIRST TO 7 (TENNIS MODE)";

        return new { team1, team2, infoBoxText, roundNumber };
    }
    
    public async Task SendTeamInfoWidgetAsync(string playerLogin)
    {
        await manialinks.SendManialinkAsync(playerLogin, WidgetTemplate, await GetManialinkData());
    }

    public async Task SendTeamInfoWidgetEveryoneAsync()
    {
        await manialinks.SendManialinkAsync(WidgetTemplate, await GetManialinkData());
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
