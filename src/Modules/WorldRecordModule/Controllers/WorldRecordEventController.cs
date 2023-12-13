using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;

namespace EvoSC.Modules.Official.WorldRecordModule.Controllers;

[Controller]
public class WorldRecordEventController(IWorldRecordService worldRecordService) : EvoScController<IEventControllerContext>
{
    [Subscribe(ModeScriptEvent.EndMapEnd)]
    public Task OnMapEndAsync(object sender, MapEventArgs mapEventArgs)
        => worldRecordService.ClearRecordAsync();

    [Subscribe(ModeScriptEvent.StartMapStart)]
    public Task OnMapStartAsync(object sender, MapEventArgs mapEventArgs)
        => worldRecordService.FetchRecordAsync(mapEventArgs.Map.Uid);
}
