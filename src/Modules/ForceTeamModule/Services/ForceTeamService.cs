using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;

namespace EvoSC.Modules.Official.ForceTeamModule.Services;

[Service]
public class ForceTeamService(IManialinkManager manialinkManager, IPlayerManagerService playerManagerService, IServerClient server) : IForceTeamService
{
    public async Task ShowWindowAsync(IPlayer player)
    {
        var players = await playerManagerService.GetOnlinePlayersAsync();
        await manialinkManager.SendManialinkAsync(player, "ForceTeamModule.ForceTeamWindow", new { players });
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
