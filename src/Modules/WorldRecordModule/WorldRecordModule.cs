using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;

namespace EvoSC.Modules.Official.WorldRecordModule;

[Module(IsInternal = true)]
public class WorldRecordModule : EvoScModule, IToggleable
{
    private readonly IWorldRecordService _worldRecordService;
    private readonly IMapService _mapService;

    public WorldRecordModule(IWorldRecordService worldRecordService, IMapService mapService)
    {
        _worldRecordService = worldRecordService;
        _mapService = mapService;
    }

    public async Task EnableAsync()
    {
        var currentMap = await _mapService.GetCurrentMapAsync();
        if (currentMap != null)
        {
            await _worldRecordService.FetchRecord(currentMap.Uid);
        }
    }

    public Task DisableAsync() => Task.CompletedTask; //do nothing
}
