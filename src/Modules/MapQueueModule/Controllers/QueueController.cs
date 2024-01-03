using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MapQueueModule.Controllers;

[Controller]
public class QueueController(IServerClient server) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.BeginMap)]
    public async Task OnEndMapAsync(object sender, MapGbxEventArgs args)
    {
        await server.Remote.ChooseNextMapAsync("Campaigns/CurrentQuarterly/Fall 2023 - 04.Map.Gbx");
    }
}
