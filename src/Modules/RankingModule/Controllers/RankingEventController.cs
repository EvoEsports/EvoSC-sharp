using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RankingModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.RankingModule.Controllers;

[Controller]
public class RankingEventController(IRankingService rankingService) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(args.Login);
        rankingService.StartSession(accountId);
        await rankingService.SendWelcomeMessageAsync(accountId);
    }

    [Subscribe(GbxRemoteEvent.PlayerDisconnect)]
    public Task OnPlayerDisconnectAsync(object sender, PlayerDisconnectGbxEventArgs args) =>
        rankingService.EndSessionAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));

    [Subscribe(GbxRemoteEvent.EndMap)]
    public async Task OnEndMapAsync(object sender, MapGbxEventArgs args)
    {
        await rankingService.SaveAllSessionsAsync();
        await rankingService.CommitMapResultAsync();
    }

    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnScoresAsync(object sender, ScoresEventArgs args) => rankingService.HandleScoresAsync(args);
}
