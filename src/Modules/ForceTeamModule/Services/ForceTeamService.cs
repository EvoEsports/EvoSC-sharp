using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;

namespace EvoSC.Modules.Official.ForceTeamModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class ForceTeamService(
    IManialinkManager manialinkManager,
    IPlayerManagerService playerManagerService,
    IServerClient server,
    Locale locale) : IForceTeamService
{
    public async Task ShowWindowAsync(IPlayer player)
    {
        var team1Info = await server.Remote.GetTeamInfoAsync(1);
        var team2Info = await server.Remote.GetTeamInfoAsync(2);
        var players = await playerManagerService.GetOnlinePlayersAsync();

        await manialinkManager.SendManialinkAsync(player, "ForceTeamModule.ForceTeamWindow",
            new { players, team1 = team1Info, team2 = team2Info, Locale = locale });
    }

    public Task BalanceTeamsAsync() => server.Remote.AutoTeamBalanceAsync();

    public async Task<PlayerTeam> SwitchPlayerAsync(IOnlinePlayer player)
    {
        if (player.IsTeam1())
        {
            await server.Remote.ForcePlayerTeamAsync(player.GetLogin(), 1);
            return PlayerTeam.Team2;
        }
        else if (player.IsTeam2())
        {
            await server.Remote.ForcePlayerTeamAsync(player.GetLogin(), 0);
            return PlayerTeam.Team1;
        }

        return PlayerTeam.Unknown;
    }
}
