using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.ForceTeamModule.Interfaces;

namespace EvoSC.Modules.Official.ForceTeamModule.Controllers;

[Controller]
public class ForceTeamManialinkController(IForceTeamService forceTeamService, IPlayerManagerService playerService) : ManialinkController
{
    public async Task SwitchPlayerAsync(string accountId)
    {
        var player = await playerService.GetOnlinePlayerAsync(accountId);

        if (player == null)
        {
            await Context.Server.ErrorMessageAsync(Context.Player,
                "Failed to find the player you are trying to switch.");
            return;
        }

        var newteam = await forceTeamService.SwitchPlayerAsync(player);
        var teamInfo = await Context.Server.Remote.GetTeamInfoAsync((int)newteam+1);
        
        await Context.Server.InfoMessageAsync(new TextFormatter()
            .AddText(player.NickName)
            .AddText(" was forced to team ")
            .AddText(teamInfo.Name, s => s.WithColor(teamInfo.RGB))
            .ToString());

        await forceTeamService.ShowWindowAsync(Context.Player);
    }
}
