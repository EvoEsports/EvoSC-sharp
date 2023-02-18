using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.FastestCp.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.FastestCp.Controllers;

[Controller]
public class FastestCpController : EvoScController<EventControllerContext>
{
    private readonly IFastestCpService _fastestCpService;

    public FastestCpController(IFastestCpService fastestCpService)
    {
        _fastestCpService = fastestCpService;
    }

    [Subscribe(ModeScriptEvent.WayPoint)]
    public void RegisterCpTime(object sender, WayPointEventArgs args)
    {
        _fastestCpService.RegisterCpTime(args);
    }

    [Subscribe(GbxRemoteEvent.EndMap)]
    public void ResetCpTimes(object sender, MapEventArgs args)
    {
        _fastestCpService.ResetCpTimes();
    }
}
