using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MapListService(IMatchSettingsService matchSettings, IPlayerRecordsService recordsService) : IMapListService
{
    public async Task<IEnumerable<IMapListMap>> GetCurrentMapsForPlayerAsync(IPlayer player)
    {
        var maps = await matchSettings.GetCurrentMapListAsync();
        
        var mapListMaps = new List<IMapListMap>();

        foreach (var map in maps)
        {
            List<IPlayerRecord> records = [];
            
            var record = await recordsService.GetPlayerRecordAsync(player, map);
            if (record != null)
            {
                records.Add(record);
            }
            
            var mapListMap = new MapListMap
            {
                Map = map,
                Records = records.OrderBy(r => r.Score),
                Tags = [] // todo: implement tags
            };
            
            mapListMaps.Add(mapListMap);
        }

        return mapListMaps;
    }
}
