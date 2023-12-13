using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;

namespace EvoSC.Modules.Official.WorldRecordModule;

[Module(IsInternal = true)]
public class WorldRecordModule(IWorldRecordService worldRecordService, IMapService mapService)
    : EvoScModule, IToggleable
{
    public async Task EnableAsync()
    {
        // initially fetch the world record to be displayed
        var currentMap = await mapService.GetCurrentMapAsync();
        
        if (currentMap != null)
        {
            await worldRecordService.FetchRecordAsync(currentMap.Uid);
        }
    }

    public Task DisableAsync() => Task.CompletedTask; //do nothing
}
