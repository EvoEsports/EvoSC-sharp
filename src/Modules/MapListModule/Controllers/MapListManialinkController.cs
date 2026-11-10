using EvoSC.Common.Controllers.Attributes;
using System.Data;
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
public class MapListManialinkController(IMapService mapService, IMapQueueService mapQueueService, IMapListService mapListService, IMapListStateService stateService, ITmxSearchService tmxSearch, IMxMapService mxMapService) : ManialinkController
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

    public async Task FavoriteMapAsync(string mapUid)
    {
        await mapListService.ToggleFavoriteAsync(Context.Player, mapUid);
        await mapListService.ShowMapListAsync(Context.Player);
    }

    public async Task SearchAsync(MapListSearchModel input)
    {
        stateService.GetFilter(Context.Player).Search = input.Search ?? "";
        await mapListService.ShowMapListAsync(Context.Player);
    }

    public async Task ToggleFilterAsync(string name)
    {
        var filter = stateService.GetFilter(Context.Player);

        switch (name)
        {
            case "all":
                stateService.ResetFilter(Context.Player);
                break;
            case "favorites":
                filter.FavoritesOnly = !filter.FavoritesOnly;
                break;
            case "search":
                filter.Search = "";
                break;
            case "disabled":
                filter.DisabledOnly = !filter.DisabledOnly;
                break;
            case "nofinish":
                filter.NoFinishOnly = !filter.NoFinishOnly;
                break;
        }

        await mapListService.ShowMapListAsync(Context.Player);
    }

    /// <summary>
    /// Add a tag to the tag filter. Selecting an active tag again removes it.
    /// </summary>
    public async Task ToggleTagAsync(string tag)
    {
        var filter = stateService.GetFilter(Context.Player);
        var removed = filter.Tags.RemoveAll(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase));

        if (removed == 0)
        {
            filter.Tags.Add(tag);
        }

        await mapListService.ShowMapListAsync(Context.Player);
    }

    /// <summary>
    /// List all maps of an author. Selecting the active author again clears it.
    /// </summary>
    public async Task ToggleAuthorAsync(string accountId)
    {
        var filter = stateService.GetFilter(Context.Player);
        filter.AuthorAccountId = filter.AuthorAccountId == accountId ? null : accountId;
        filter.AuthorName = null;
        await mapListService.ShowMapListAsync(Context.Player);
    }

    /// <summary>
    /// Sort by a column. Selecting the active column again flips the direction.
    /// </summary>
    public async Task SortAsync(string column)
    {
        if (!Enum.TryParse<MapListSort>(column, true, out var sort))
        {
            return;
        }

        var filter = stateService.GetFilter(Context.Player);

        if (filter.Sort == sort)
        {
            filter.Descending = !filter.Descending;
        }
        else
        {
            filter.Sort = sort;
            filter.Descending = false;
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
    public async Task ToggleMapEnabledAsync(string mapUid)
    {
        var enabled = await mapListService.ToggleMapEnabledAsync(Context.Player, mapUid);

        if (enabled == null)
        {
            Context.AuditEvent.Error();
            return;
        }

        Context.AuditEvent
            .WithEventName(enabled.Value ? AuditEvents.EnableMap : AuditEvents.DisableMap)
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
        var tmx = stateService.GetTmxState(Context.Player);

        if (tmx.Result.Maps.Count == 0)
        {
            await LoadTmxPageAsync(tmx, tmx.Query, 0);
        }

        // hide the map list first so it doesn't flash while the dialog is being drawn
        await HideAsync(Context.Player, "MapListModule.MapList");
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxSearchAsync(TmxSearchModel input)
    {
        await LoadTmxPageAsync(stateService.GetTmxState(Context.Player), input.TmxSearch ?? "", 0);
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxPageAsync(int page)
    {
        var tmx = stateService.GetTmxState(Context.Player);
        await LoadTmxPageAsync(tmx, tmx.Query, Math.Max(page, 0));
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxSelectAsync(int id)
    {
        var tmx = stateService.GetTmxState(Context.Player);
        tmx.SelectedId = id;
        tmx.Message = null;
        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    [ManialinkRoute(Permission = MapsPermissions.AddMap)]
    public async Task TmxAddAsync(int id)
    {
        var tmx = stateService.GetTmxState(Context.Player);

        try
        {
            var tmxInfo = tmx.Result.Maps.FirstOrDefault(m => m.Id == id);
            var existing = await mapService.GetMapByExternalIdAsync(id.ToString()) ??
                           (string.IsNullOrEmpty(tmxInfo?.Uid) ? null : await mapService.GetMapByUidAsync(tmxInfo.Uid));

            var map = existing == null ? await mxMapService.FindAndDownloadMapAsync(id, null, Context.Player) : null;

            if (existing != null)
            {
                tmx.Message = "Map already in map list.";
                Context.AuditEvent.Error();
            }
            else if (map == null)
            {
                tmx.Message = "Could not download the map from Trackmania Exchange.";
                Context.AuditEvent.Error();
            }
            else
            {
                // carry over the Trackmania Exchange tags as map list tags
                var tmxMap = tmx.Result.Maps.FirstOrDefault(m => m.Id == id);

                foreach (var tag in tmxMap?.Tags ?? [])
                {
                    await mapListService.AddTagAsync(map.Uid, tag);
                }

                tmx.Message = $"Added '{map.Name}'.";
                Context.AuditEvent.WithEventName(AuditEvents.AddTmxMap).HavingProperties(new { id, map.Uid }).Success();
            }
        }
        catch (DuplicateNameException)
        {
            tmx.Message = "That map has already been added.";
            Context.AuditEvent.Error();
        }
        catch (FileLoadException)
        {
            tmx.Message = "The server can't find the downloaded map file. Check the maps path in the EvoSC config.";
            Context.AuditEvent.Error();
            await mapListService.ShowAddMapDialogAsync(Context.Player);
            throw;
        }
        catch (Exception)
        {
            tmx.Message = "Failed to add the map.";
            Context.AuditEvent.Error();
            await mapListService.ShowAddMapDialogAsync(Context.Player);
            throw;
        }

        await mapListService.ShowAddMapDialogAsync(Context.Player);
    }

    public async Task CloseAddMapAsync()
    {
        await HideAsync(Context.Player, "MapListModule.Dialogs.AddMapDialog");
        await mapListService.ShowMapListAsync(Context.Player);
    }

    private async Task LoadTmxPageAsync(TmxSearchState tmx, string query, int page)
    {
        var result = await tmxSearch.SearchAsync(query, page);

        tmx.Query = query;
        tmx.Message = null;

        if (result == null)
        {
            tmx.Message = "Could not reach Trackmania Exchange.";
            return;
        }

        tmx.Page = page;
        tmx.Result = result;
        tmx.SelectedId = result.Maps.FirstOrDefault()?.Id;
    }
}
