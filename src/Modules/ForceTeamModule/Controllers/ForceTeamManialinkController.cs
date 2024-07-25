using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.ForceTeamModule.Events;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;
using EvoSC.Modules.Official.ForceTeamModule.Permissions;

namespace EvoSC.Modules.Official.ForceTeamModule.Controllers;

[Controller]
public class ForceTeamManialinkController(IForceTeamService forceTeamService, IPlayerManagerService playerService) : ManialinkController
{
    [ManialinkRoute(Permission = ForceTeamPermissions.ForcePlayerTeam)]
    public async Task SwitchPlayerAsync(string accountId)
    {
        var player = await playerService.GetOnlinePlayerAsync(accountId);

        if (player == null)
        {
            await Context.Server.ErrorMessageAsync(Context.Player,
                "Failed to find the player you are trying to switch.");
            return;
        }

        var newTeam = await forceTeamService.SwitchPlayerAsync(player);
        var teamInfo = await Context.Server.Remote.GetTeamInfoAsync((int)newTeam + 1);
        
        await Context.Server.InfoMessageAsync(new TextFormatter()
            .AddText(player.NickName)
            .AddText(" was forced to team ")
            .AddText(teamInfo.Name, s => s.WithColor(teamInfo.RGB))
            .ToString());

        Context.AuditEvent
            .WithEventName(AuditEvents.PlayerSwitched)
            .HavingProperties(new { player })
            .Success();
        
        await forceTeamService.ShowWindowAsync(Context.Player);
    }
}
