using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.TeamInfoModule.Controllers;

[Controller]
public class TeamInfoEventController(ITeamInfoService teamInfoService) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args)
    {
        await teamInfoService.SendTeamInfoWidgetAsync(args.Login);
    }

    [Subscribe(GbxRemoteEvent.EndMap)]
    public async Task OnEndMap(object sender)
    {
        await teamInfoService.HideTeamInfoWidgetEveryoneAsync();
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMap(object sender)
    {
        await teamInfoService.SendTeamInfoWidgetEveryoneAsync();
    }
}
