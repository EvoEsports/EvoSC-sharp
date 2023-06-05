using System.Data.SqlServerCe;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Services;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.CurrentMapModule.Controllers;

[Controller]
public class CurrentMapController : EvoScController<EventControllerContext>
{
    private readonly ICurrentMapService _service;
    private readonly ILogger<CurrentMapController> _logger;

    public CurrentMapController(ICurrentMapService service, ILogger<CurrentMapController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [Subscribe(GbxRemoteEvent.BeginMatch)]
    public Task OnBeginMatch(object sender, EventArgs args)
    {
        return _service.ShowWidget();
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMap(object sender, MapGbxEventArgs args)
    {
        return _service.ShowWidget(args);
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStart(object sender, PodiumEventArgs args)
    {
        return _service.HideWidget();
    }
}
