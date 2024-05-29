using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.MapListModule.Events;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using EvoSC.Modules.Official.MapsModule;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListManialinkController(IMapService mapService, IMapQueueService mapQueueService, IServerClient server, IMapListService mapList) : ManialinkController
{
    public async Task QueueMapAsync(string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        await mapQueueService.EnqueueAsync(map);
    }
    
    public async Task DropMapAsync(string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        await mapQueueService.DropAsync(map);
    }

    public Task FavoriteMapAsync(string mapUid)
    {
        Console.WriteLine("map favorited! (surely)");
        return Task.CompletedTask;
    }

    [ManialinkRoute(Permission = MapsPermissions.RemoveMap)]
    public async Task DeleteMapAsync(string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        
        await ShowAsync(Context.Player, "MapListModule.Dialogs.ConfirmDeleteDialog", new
        {
            mapName = map.Name,
            mapUid = map.Uid
        });

        Context.AuditEvent
            .WithEventName(AuditEvents.RemoveMapConfirm)
            .HavingProperties(new { map })
            .Success();
    }
    
    [ManialinkRoute(Permission = MapsPermissions.RemoveMap)]
    public async Task ConfirmDeleteAsync(string mapUid, bool confirmed)
    {
        await HideAsync(Context.Player, "MapListModule.Dialogs.ConfirmDeleteDialog");
        
        if (!confirmed)
        {
            Context.AuditEvent.Cancel();
            return;
        }

        var map = await mapService.GetMapByUidAsync(mapUid);

        Context.AuditEvent
            .WithEventName(AuditEvents.RemoveMap)
            .HavingProperties(new { map });
        
        try
        {
            await mapService.RemoveMapAsync(map.Id);

            await server.SuccessMessageAsync(Context.Player, $"'{map.Name}' was removed from the map list.");
            Context.AuditEvent.Success();

            // todo: add this to a service instead
            var maps = await mapList.GetCurrentMapsForPlayerAsync(Context.Player);
        
            await ShowAsync(Context.Player, "MapListModule.MapList", new
            {
                maps = maps
            });
        }
        catch (Exception ex)
        {
            await server.ErrorMessageAsync(Context.Player, $"Failed to remove the map '{map.Name}'");
            Context.AuditEvent.Error();
        }
    }       
}
