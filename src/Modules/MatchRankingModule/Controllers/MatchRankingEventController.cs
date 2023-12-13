using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MatchRankingModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MatchRankingModule.Controllers;

[Controller]
public class MatchRankingEventController(IMatchRankingService matchRankingService) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScores(object data, ScoresEventArgs eventArgs)
    {
        if (eventArgs.Section is ModeScriptSection.EndMatch)
        {
            await matchRankingService.ResetMatchData();
            await matchRankingService.HideManialink();
            return;
        }
        
        await matchRankingService.UpdateAndShowScores(eventArgs);
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnBeginMapAsync(object sender, RoundEventArgs args)
    {
        await matchRankingService.SendManialinkToPlayers();
    }

    [Subscribe(ModeScriptEvent.StartMatchStart)]
    public async Task OnStartMatch(object sender, MatchEventArgs args)
    {
        await matchRankingService.ResetMatchData();
        await matchRankingService.SendManialinkToPlayers();
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public async Task OnPodiumStart(object sender, PodiumEventArgs args)
    {
        await matchRankingService.HideManialink();
    }

    [Subscribe(GbxRemoteEvent.PlayerInfoChanged)]
    public async Task OnPlayerInfoChanged(object sender, PlayerInfoChangedGbxEventArgs args)
    {
        await matchRankingService.HandlePlayerStateChange(PlayerUtils.ConvertLoginToAccountId(args.PlayerInfo.Login));
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args)
    {
        await matchRankingService.SendManialinkToPlayer(PlayerUtils.ConvertLoginToAccountId(args.Login));
    }
}
