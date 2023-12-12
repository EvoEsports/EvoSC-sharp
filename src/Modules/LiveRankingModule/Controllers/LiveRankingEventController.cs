using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.LiveRankingModule.Controllers;

[Controller]
public class LiveRankingEventController : EvoScController<IEventControllerContext>
{
    private readonly ILiveRankingService _service;

    public LiveRankingEventController(ILiveRankingService service) => _service = service;

    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task OnPlayerWaypointAsync(object sender, WayPointEventArgs args) => _service.OnPlayerWaypointAsync(args);

    [Subscribe(ModeScriptEvent.GiveUp)]
    public Task OnPlayerGiveUpAsync(object sender, PlayerUpdateEventArgs args) => _service.OnPlayerGiveupAsync(args);

    [Subscribe(ModeScriptEvent.StartRoundStart)]
    public Task OnStartRoundAsync(object sender, RoundEventArgs args) => _service.OnStartRoundAsync(args);

    [Subscribe(ModeScriptEvent.EndMapStart)]
    public Task OnEndMapAsync(object sender, MapEventArgs args) => _service.OnEndMapAsync(args);

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync(object sender, PodiumEventArgs args) => _service.OnPodiumStartAsync(args);

    [Subscribe(ModeScriptEvent.EndRoundStart)]
    public Task OnEndRoundAsync(object sender, RoundEventArgs args) => _service.OnEndRoundAsync(args);

    [Subscribe(GbxRemoteEvent.BeginMatch)]
    public Task OnBeginMatchAsync(object sender, EventArgs args) => _service.OnBeginMatchAsync();

    [Subscribe(GbxRemoteEvent.EndMatch)]
    public Task OnEndMatchAsync(object sender, EndMatchGbxEventArgs args) => _service.OnEndMatchAsync(args);

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args) => _service.SendManialinkAsync();
}
