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
    [Subscribe(ModeScriptEvent.Scores)]
    public async Task OnScores(object sender, ScoresEventArgs args)
    {
        if (args.Section is not (ModeScriptSection.EndRound or ModeScriptSection.Undefined))
        {
            return;
        }

        await service.HandleScoresAsync(args);
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMap(object sender, MapGbxEventArgs args)
        => service.InitializeAsync();

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task HideWidgetOnPodium(object sender, PodiumEventArgs args)
        => service.HideWidgetAsync();
}
