using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardEventController(IScoreboardService scoreboardService, IScoreboardNicknamesService nicknamesService) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.EndMapEnd)]
    public async Task OnEndMapAsync(object sender, MapEventArgs args)
    {
        await nicknamesService.ClearNicknamesAsync();
    }
    
    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        await nicknamesService.LoadNicknamesAsync();
        await scoreboardService.LoadAndSendRequiredAdditionalInfoAsync();
        await nicknamesService.SendNicknamesManialinkAsync();
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

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerGbxEventArgs args)
    {
        await nicknamesService.AddNicknameByLoginAsync(args.Login);
    }
}
