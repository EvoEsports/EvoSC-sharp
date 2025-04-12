﻿using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using EvoSC.Modules.Official.SetNameModule.Events;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardEventController(
    IScoreboardService scoreboardService,
    IScoreboardStateService scoreboardStateService,
    IScoreboardNicknamesService nicknamesService
)
    : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnectAsync(object sender, PlayerGbxEventArgs args) =>
        nicknamesService.FetchAndAddNicknameByLoginAsync(args.Login);

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        await nicknamesService.InitializeNicknamesAsync();
        await scoreboardStateService.SetCurrentRoundNumberAsync(1);
        await scoreboardService.SendScoreboardAsync();
    }

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public async Task OnStartRoundAsync(object sender, RoundEventArgs args)
    {
        await scoreboardStateService.SetCurrentRoundNumberAsync(args.Count);
        await scoreboardService.SendMetaDataAsync();
    }

    [Subscribe(ModeScriptEvent.EndRoundEnd)]
    public async Task OnEndRoundAsync(object sender, RoundEventArgs args)
    {
        await scoreboardStateService.SetCurrentRoundNumberAsync(args.Count + 1);
        await scoreboardService.SendMetaDataAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpStart)]
    public async Task OnWarmUpStartAsync(object sender, EventArgs args)
    {
        await scoreboardStateService.SetCurrentRoundNumberAsync(1);
        await scoreboardService.SendMetaDataAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpEnd)]
    public async Task OnWarmUpEndAsync(object sender, EventArgs args)
    {
        await scoreboardStateService.SetCurrentRoundNumberAsync(1);
        await scoreboardService.SendMetaDataAsync();
    }

    [Subscribe(ModeScriptEvent.WarmUpStartRound)]
    public async Task OnWarmUpStartRoundAsync(object sender, WarmUpRoundEventArgs args)
    {
        await scoreboardStateService.SetCurrentRoundNumberAsync(args.Current);
        await scoreboardService.SendMetaDataAsync();
    }

    [Subscribe(SetNameEvents.NicknameUpdated)]
    public async Task OnPlayerNicknameChangeAsync(object sender, NicknameUpdatedEventArgs args)
    {
        await nicknamesService.OverwriteNicknameAsync(args.Player.GetLogin(), args.NewName);
        await nicknamesService.SendNicknamesManialinkAsync();
    }
}
