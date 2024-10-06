using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardEventController(
    IScoreboardService scoreboardService,
    IScoreboardNicknamesService nicknamesService
)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnectAsync(object sender, PlayerGbxEventArgs args) =>
        nicknamesService.AddNicknameByLoginAsync(args.Login);

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        await nicknamesService.LoadNicknamesAsync();
        await scoreboardService.SendScoreboardAsync();
    }
}
