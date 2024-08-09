using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.LiveRankingModule.Controllers;

[Controller]
public class LiveRankingEventController(ILiveRankingService service) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMap(object sender, MapGbxEventArgs args)
        => service.DetectModeAndRequestScoreAsync();
    
    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScores(object sender, ScoresEventArgs args)
    {
        if (args.Section is not (ModeScriptSection.EndRound or ModeScriptSection.Undefined))
        {
            return;
        }

        await service.MapScoresAndSendWidgetAsync(args);
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStart(object sender, PodiumEventArgs args)
        => service.HideWidgetAsync();

    [Subscribe(ModeScriptEvent.WayPoint)]
    public async Task OnWayPoint(object sender, WayPointEventArgs args)
    {
        if (!args.IsEndLap)
        {
            return;
        }

        if (await service.CurrentModeIsPointsBasedAsync())
        {
            return;
        }

        await service.RequestScoresAsync();
    }
}
