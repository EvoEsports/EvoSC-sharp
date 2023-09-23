using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;

namespace EvoSC.Modules.Official.WorldRecordModule.Controllers;

[Controller]
public class WorldRecordEventController : EvoScController<IEventControllerContext>
{
    private readonly IWorldRecordService _worldRecordService;

    public WorldRecordEventController(IWorldRecordService worldRecordService)
    {
        _worldRecordService = worldRecordService;
    }

    //[Subscribe(ModeScriptEvent.Scores)]
    //public Task OnScores(object sender, ScoresEventArgs scoresEventArgs)
    //    => _worldRecordService.DetectNewWorldRecordThroughScores(scoresEventArgs);

    [Subscribe(ModeScriptEvent.EndMapEnd)]
    public Task OnMapEnd(object sender, MapEventArgs mapEventArgs)
        => _worldRecordService.ClearRecord();

    [Subscribe(ModeScriptEvent.StartMapStart)]
    public Task OnMapStart(object sender, MapEventArgs mapEventArgs)
        => _worldRecordService.FetchRecord(mapEventArgs.Map.Uid);
}
