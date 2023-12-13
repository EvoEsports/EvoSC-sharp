using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.Scoreboard.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardEventController(IScoreboardService scoreboardService) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        await scoreboardService.LoadAndSendRequiredAdditionalInfoAsync();
        await scoreboardService.ShowScoreboardToAllAsync();
    }

    [Subscribe(MatchSettingsEvent.MatchSettingsLoaded)]
    public async Task OnMatchSettingsLoadedAsync(object sender, MatchSettingsLoadedEventArgs args)
    {
        await scoreboardService.LoadAndSendRequiredAdditionalInfoAsync();
        await scoreboardService.ShowScoreboardToAllAsync();
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnRoundStartAsync(object sender, RoundEventArgs args)
    {
        scoreboardService.SetCurrentRound(args.Count);
        await scoreboardService.SendRequiredAdditionalInfoAsync();
    }
}
