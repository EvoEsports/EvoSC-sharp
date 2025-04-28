using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Controllers;

[Controller]
public class ServerSyncController(ISyncService syncService) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public Task OnLocalChatMessageAsync(object sender, PlayerChatGbxEventArgs args) =>
        syncService.PublishChatMessageAsync(args);

    [Subscribe(GbxRemoteEvent.EndMap)]
    public Task OnEndMapAsync(object sender, MapGbxEventArgs args) =>
        syncService.PublishMapFinishedAsync();

    [Subscribe(ModeScriptEvent.EndRoundStart)]
    public Task OnEndRoundAsync(object sender, RoundEventArgs args) =>
        syncService.PublishEndRoundAsync();

    [Subscribe(GbxRemoteEvent.EndMatch)]
    public Task OnEndMatchAsync(object sender, EndMatchGbxEventArgs args) =>
        syncService.PublishEndMatchAsync();

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnWayPointAsync(object sender, WayPointEventArgs args) => syncService.PublishWaypointAsync(args);

    [Subscribe(ModeScriptEvent.Scores)]
    public Task OnScoresAsync(object sender, ScoresEventArgs args) => syncService.PublishScoresAsync(args);
}
