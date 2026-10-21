using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GroupsModule.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using EvoSC.Modules.Official.ScoreboardModule.Utils;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ScoreboardModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardRolesService(
    IPlayerManagerService playerManagerService,
    IGroupRolesService groupRolesService,
    IManialinkManager manialinkManager,
    ILogger<ScoreboardRolesService> logger) : IScoreboardRolesService
{
    public async Task SendRolesManialinkAsync()
    {
        var onlinePlayers = (await playerManagerService.GetOnlinePlayersAsync()).ToList();
        var roles = await groupRolesService.GetRolesAsync(onlinePlayers);

        var rolesByLogin = onlinePlayers
            .Where(player => roles.ContainsKey(player.AccountId))
            .ToDictionary(player => player.GetLogin(), player => ScoreboardRoleUtils.FormatRole(roles[player.AccountId]));

        logger.LogInformation("Sending scoreboard roles: {RoleCount} of {PlayerCount} online players have a role",
            rolesByLogin.Count, onlinePlayers.Count);

        await manialinkManager.SendPersistentManialinkAsync("ScoreboardModule.PlayerRoles",
            new { roles = ScoreboardRoleUtils.ToManiaScriptArray(rolesByLogin) });
    }
}
