using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardEventController(IScoreboardNicknamesService nicknamesService)
    : EvoScController<IEventControllerContext>
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
        await nicknamesService.SendNicknamesManialinkAsync();
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerGbxEventArgs args)
    {
        await nicknamesService.AddNicknameByLoginAsync(args.Login);
    }
}
