using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;
using EvoSC.Modules.Official.MapsModule;
using EvoSC.Modules.Official.MapsModule.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MapListModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MapListService(
    IMatchSettingsService matchSettings,
    IPlayerRecordsService recordsService,
    IContextService context,
    IMapService mapService,
    ILogger<MapListService> logger,
    IServerClient serverClient,
    IManialinkManager manialinkManager,
    IPermissionManager permissions) : IMapListService
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
                Tags = [] // todo: implement tags - https://github.com/EvoEsports/EvoSC-sharp/issues/177
            };

            mapListMaps.Add(mapListMap);
        }

        return mapListMaps;
    }

    public async Task DeleteMapAsync(IPlayer actor, string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);

        context.Audit().WithEventName(AuditEvents.MapRemoved)
            .HavingProperties(new { map });

        if (map == null)
        {
            context.Audit().Error();
            logger.LogError("Map with UID {MapUid} was not found", mapUid);
            return;
        }

        try
        {
            await mapService.RemoveMapAsync(map.Id);
            context.Audit().Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to Remove map");
            context.Audit().Error();

            await serverClient.ErrorMessageAsync(actor, $"Failed to remove the map '{map.Name}'");
            return;
        }
        
        await serverClient.SuccessMessageAsync(actor, $"'{map.Name}' was removed from the map list.");
    }

    public async Task ShowMapListAsync(IPlayer player)
    {
        var canRemoveMaps = await permissions.HasPermissionAsync(player, MapsPermissions.RemoveMap);
        var maps = await GetCurrentMapsForPlayerAsync(player);
        await manialinkManager.SendManialinkAsync(player, "MapListModule.MapList", new { maps, canRemoveMaps });
    }

    public Task ConfirmMapDeletionsAsync(IPlayer player, IMap map) => 
        manialinkManager.SendManialinkAsync(player, "MapListModule.Dialogs.ConfirmDeleteDialog", new
        {
            mapName = map.Name,
            mapUid = map.Uid
        });
}
