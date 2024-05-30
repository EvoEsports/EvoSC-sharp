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
public class MapListManialinkController(IMapService mapService, IMapQueueService mapQueueService, IMapListService mapListService) : ManialinkController
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
        // todo: favorite maps https://github.com/EvoEsports/EvoSC-sharp/issues/177
        Console.WriteLine("map favorited! (surely)");
        return Task.CompletedTask;
    }

    [ManialinkRoute(Permission = MapsPermissions.RemoveMap)]
    public async Task DeleteMapAsync(string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        await mapListService.ConfirmMapDeletionsAsync(Context.Player, map);

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

        await mapListService.DeleteMapAsync(Context.Player, mapUid);
        await mapListService.ShowMapListAsync(Context.Player);
    }
}
