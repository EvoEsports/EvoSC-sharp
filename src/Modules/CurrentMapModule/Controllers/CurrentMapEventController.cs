﻿using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.CurrentMapModule.Controllers;

[Controller]
public class CurrentMapEventController(ICurrentMapService service) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.BeginMatch)]
    public Task OnBeginMatchAsync(object sender, EventArgs args)
    {
        return service.ShowWidgetAsync();
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMapAsync(object sender, MapGbxEventArgs args)
    {
        return service.ShowWidgetAsync(args);
    }

    [Subscribe(ModeScriptEvent.PodiumStart)]
    public Task OnPodiumStartAsync(object sender, PodiumEventArgs args)
    {
        return service.HideWidgetAsync();
    }
}
