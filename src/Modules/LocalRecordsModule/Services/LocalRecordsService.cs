using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Database;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

namespace EvoSC.Modules.Official.LocalRecordsModule.Services;

[Service]
public class LocalRecordsService(IMapService mapService, ILocalRecordRepository localRecordRepository) : ILocalRecordsService
{
    public async Task<IEnumerable<ILocalRecord>> GetLocalsOfCurrentMapAsync()
    {
        var currentMap = await mapService.GetCurrentMapAsync();

        if (currentMap == null)
        {
            throw new InvalidOperationException("Failed to get current map");
        }
        
        return await localRecordRepository.GetLocalRecordsOfMapByIdAsync(currentMap.Id);
    }
}
