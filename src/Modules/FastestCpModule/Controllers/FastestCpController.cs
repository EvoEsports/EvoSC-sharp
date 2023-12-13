using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.FastestCpModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.FastestCpModule.Controllers;

[Controller]
public class FastestCpController(IFastestCpService fastestCpService) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.WayPoint)]
    public Task RegisterCpTimeAsync(object sender, WayPointEventArgs args)
    {
        return fastestCpService.RegisterCpTimeAsync(args);
    }

    [Subscribe(GbxRemoteEvent.EndMap)]
    public Task ResetCpTimesAsync(object sender, MapGbxEventArgs args)
    {
        return fastestCpService.ResetCpTimesAsync();
    }
}
