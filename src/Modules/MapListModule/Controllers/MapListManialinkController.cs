using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.MapListModule.Events;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Models;
using EvoSC.Modules.Official.MapListModule.Permissions;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using EvoSC.Modules.Official.MapsModule;
using EvoSC.Modules.Official.MapsModule.Interfaces;

namespace EvoSC.Modules.Official.MapListModule.Controllers;

[Controller]
public class MapListManialinkController(IMapService mapService, IMapQueueService mapQueueService, IMapListService mapListService, IMapListStateService stateService, IMapListFilterService filterService, IMapListTmxService tmxService) : ManialinkController
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

    public async Task SetFavoriteAsync(string mapUid, bool favorite)
    {
        await mapListService.SetFavoriteAsync(Context.Player, mapUid, favorite);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    public async Task SearchAsync(MapListSearchModel input)
    {
        filterService.SetSearch(Context.Player, input.Search);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    public async Task ResetFilterAsync()
    {
        stateService.ResetFilter(Context.Player);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    public async Task ClearSearchAsync()
    {
        filterService.SetSearch(Context.Player, null);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    /// <summary>
    /// Turn one of the filters (favorites, nofinish or disabled) on or off.
    /// </summary>
    public async Task SetFilterAsync(string name, bool enabled)
    {
        filterService.SetFilter(Context.Player, name, enabled);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    /// <summary>
    /// Add a tag to the tag filter or remove it.
    /// </summary>
    public async Task SetTagAsync(string tag, bool active)
    {
        filterService.SetTag(Context.Player, tag, active);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    /// <summary>
    /// List all maps of an author, or clear the author filter.
    /// </summary>
    public async Task SetAuthorAsync(string accountId, bool active)
    {
        filterService.SetAuthor(Context.Player, accountId, active);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    /// <summary>
    /// Sort by a column in the given direction.
    /// </summary>
    public async Task SortAsync(string column, bool descending)
    {
        if (!filterService.SetSort(Context.Player, column, descending))
        {
            return;
        }

        await mapListService.ShowMapListAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapListPermissions.ManageTags)]
    public async Task OpenTagsAsync(string mapUid)
    {
        stateService.SetTagMapUid(Context.Player, mapUid);
        await mapListService.ShowTagsDialogAsync(Context.Player, mapUid);
    }

    public async Task ViewTagsAsync(string mapUid)
    {
        await mapListService.ShowTagsViewDialogAsync(Context.Player, mapUid);
    }

    [ManialinkRoute(Permission = MapListPermissions.ManageTags)]
    public async Task AddTagAsync(MapTagModel input)
    {
        var mapUid = stateService.GetTagMapUid(Context.Player);

        if (mapUid == null || string.IsNullOrWhiteSpace(input.Tag) ||
            !await mapListService.AddTagAsync(mapUid, input.Tag))
        {
            Context.AuditEvent.Error();
            return;
        }

        Context.AuditEvent.WithEventName(AuditEvents.AddTag).HavingProperties(new { mapUid, input.Tag }).Success();
        await RefreshTagsAsync(mapUid);
    }

    [ManialinkRoute(Permission = MapListPermissions.ManageTags)]
    public async Task RemoveTagAsync(string tag)
    {
        var mapUid = stateService.GetTagMapUid(Context.Player);

        if (mapUid == null || !await mapListService.RemoveTagAsync(mapUid, tag))
        {
            Context.AuditEvent.Error();
            return;
        }

        Context.AuditEvent.WithEventName(AuditEvents.RemoveTag).HavingProperties(new { mapUid, tag }).Success();
        await RefreshTagsAsync(mapUid);
    }

    public async Task CloseTagsAsync()
    {
        stateService.SetTagMapUid(Context.Player, null);
        await HideAsync(Context.Player, "MapListModule.Dialogs.MapTagsDialog");
    }

    private async Task RefreshTagsAsync(string mapUid)
    {
        await mapListService.ShowMapListAsync(Context.Player);
        await mapListService.ShowTagsDialogAsync(Context.Player, mapUid);
    }

    [ManialinkRoute(Permission = MapListPermissions.ToggleMapEnabled)]
    public async Task SetMapEnabledAsync(string mapUid, bool enabled)
    {
        if (!await mapListService.SetMapEnabledAsync(Context.Player, mapUid, enabled))
        {
            Context.AuditEvent.Error();
            return;
        }

        Context.AuditEvent
            .WithEventName(enabled ? AuditEvents.EnableMap : AuditEvents.DisableMap)
            .HavingProperties(new { mapUid })
            .Success();

        await mapListService.ShowMapListAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.RemoveMap)]
    public async Task DeleteMapAsync(string mapUid)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        await mapListService.ConfirmMapDeletionAsync(Context.Player, map);

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
        
        Context.AuditEvent.WithEventName(AuditEvents.RemoveMap).Success();
        
        await mapListService.ShowMapListAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task OpenAddMapAsync()
    {
        await tmxService.EnsureLoadedAsync(Context.Player);

        // hide the map list first so it doesn't flash while the dialog is being drawn
        await HideAsync(Context.Player, "MapListModule.MapList");
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxSearchAsync(TmxSearchModel input)
    {
        await tmxService.SearchAsync(Context.Player, input.TmxSearch ?? "");
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxPageAsync(int page)
    {
        await tmxService.GoToPageAsync(Context.Player, page);
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxSelectAsync(int id)
    {
        tmxService.Select(Context.Player, id);
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxAddAsync(int id)
    {
        string? mapUid;

        try
        {
            mapUid = await tmxService.AddMapAsync(Context.Player, id);
        }
        catch (Exception)
        {
            Context.AuditEvent.Error();
            await mapListService.ShowAddMapDialogAsync(Context.Player);
            throw;
        }

        if (mapUid == null)
        {
            Context.AuditEvent.Error();
        }
        else
        {
            Context.AuditEvent.WithEventName(AuditEvents.AddTmxMap).HavingProperties(new { id, Uid = mapUid }).Success();
        }

        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    public async Task CloseAddMapAsync()
    {
        await HideAsync(Context.Player, "MapListModule.Dialogs.AddMapDialog");
        await mapListService.ShowMapListAsync(Context.Player);
    }
}
