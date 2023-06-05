using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.CurrentMapModule.Controllers;

[Controller]
public class CurrentMapController : EvoScController<EventControllerContext>
{
    private readonly ICurrentMapService _service;

    public CurrentMapController(ICurrentMapService service)
    {
        _service = service;
    }

    [Subscribe(GbxRemoteEvent.BeginMatch)]
    public Task OnBeginMatchAsync(object sender, EventArgs args)
    {
        return _service.ShowWidgetAsync();
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        return _service.ShowWidgetAsync(args);
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync(object sender, PodiumEventArgs args)
    {
        return _service.HideWidgetAsync();
    }
}
