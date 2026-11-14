using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Interfaces.Database;
using EvoSC.Modules.Official.MapListModule.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;
using EvoSC.Modules.Official.MapListModule.Permissions;
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
    IChatService chat,
    IManialinkManager manialinkManager,
    IPermissionManager permissions,
    IMapListRepository repository,
    IMapListStateService state,
    IServerClient server) : IMapListService
{
    public async Task<IEnumerable<IMapListMap>> GetCurrentMapsForPlayerAsync(IPlayer player)
    {
        var maps = await matchSettings.GetCurrentMapListAsync();
        var favorites = await repository.GetFavoriteMapIdsAsync(player.Id);
        var tags = await repository.GetTagsByMapAsync();

        var mapListMaps = new List<IMapListMap>();

        foreach (var map in maps)
        {
            List<IPlayerRecord> records = [];

            var record = await recordsService.GetPlayerRecordAsync(player, map);
            if (record != null)
            {
                records.Add(record);
            }

            mapListMaps.Add(new MapListMap
            {
                Map = map,
                Records = records.OrderBy(r => r.Score),
                Tags = tags.TryGetValue(map.Id, out var mapTags) ? mapTags : [],
                IsFavorite = favorites.Contains(map.Id)
            });
        }

        return mapListMaps;
    }

    public async Task<MapListResult> GetFilteredMapsForPlayerAsync(IPlayer player, MapListFilter filter)
    {
        var canSeeDisabled = await permissions.HasPermissionAsync(player, MapListPermissions.ToggleMapEnabled);
        var all = (await GetCurrentMapsForPlayerAsync(player)).ToList();

        if (canSeeDisabled)
        {
            all.AddRange(await GetDisabledMapsForPlayerAsync(player, all));
        }
        else
        {
            all.RemoveAll(m => !m.IsEnabled);
        }

        var allTags = await repository.GetAllTagsAsync();
        IEnumerable<IMapListMap> maps = all;

        var search = filter.Search.Trim();
        if (search.Length > 0)
        {
            maps = maps.Where(m =>
                Contains(m.Map.Name, search) ||
                Contains(m.Map.Author?.NickName, search) ||
                Contains(m.Map.Author?.AccountId, search));
        }

        if (filter.DisabledOnly && canSeeDisabled)
        {
            maps = maps.Where(m => !m.IsEnabled);
        }

        if (filter.FavoritesOnly)
        {
            maps = maps.Where(m => m.IsFavorite);
        }

        if (filter.NoFinishOnly)
        {
            maps = maps.Where(m => !m.HasPb);
        }

        foreach (var filterTag in filter.Tags)
        {
            maps = maps.Where(m => m.Tags.Any(t => t.Name.Equals(filterTag, StringComparison.OrdinalIgnoreCase)));
        }

        if (filter.AuthorAccountId != null)
        {
            filter.AuthorName ??= all
                .Select(m => m.Map.Author)
                .FirstOrDefault(a => a?.AccountId == filter.AuthorAccountId)?.NickName;
            maps = maps.Where(m => m.Map.Author?.AccountId == filter.AuthorAccountId);
        }

        var sorted = Sort(maps, filter).ToList();
        return new MapListResult(sorted, all.Count, allTags);
    }

    /// <summary>
    /// Get disabled maps, which are not part of the server's map rotation, but still exist in the database.
    /// </summary>
    private async Task<List<IMapListMap>> GetDisabledMapsForPlayerAsync(IPlayer player,
        IReadOnlyCollection<IMapListMap> inRotation)
    {
        var rotationUids = inRotation.Select(m => m.Map.Uid).ToHashSet();
        var disabled = (await mapService.GetCurrentMapListAsync())
            .Where(m => !m.Enabled && !rotationUids.Contains(m.Uid))
            .ToList();

        var favorites = await repository.GetFavoriteMapIdsAsync(player.Id);
        var tags = await repository.GetTagsByMapAsync();
        var result = new List<IMapListMap>();

        foreach (var map in disabled)
        {
            var record = await recordsService.GetPlayerRecordAsync(player, map);

            result.Add(new MapListMap
            {
                Map = map,
                Records = record != null ? [record] : [],
                Tags = tags.TryGetValue(map.Id, out var mapTags) ? mapTags : [],
                IsFavorite = favorites.Contains(map.Id)
            });
        }

        return result;
    }

    public async Task<bool> SetMapEnabledAsync(IPlayer actor, string mapUid, bool enable)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);

        if (map == null)
        {
            logger.LogError("Map with UID {MapUid} was not found", mapUid);
            return false;
        }

        if (map.Enabled == enable)
        {
            // the list the player acted on was outdated
            return true;
        }

        try
        {
            await mapService.SetMapEnabledAsync(map, enable);

            await matchSettings.EditCurrentMatchSettingsAsync(ms =>
            {
                ms.Maps.RemoveAll(m => m.Uid == map.Uid);

                if (enable)
                {
                    ms.AddMap(map);
                }
            });

            if (enable)
            {
                await server.Remote.AddMapAsync(map.FilePath);
            }
            else
            {
                await server.Remote.RemoveMapAsync(map.FilePath);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to {Action} map {MapUid}", enable ? "enable" : "disable", mapUid);
            await chat.ErrorMessageAsync($"Failed to {(enable ? "enable" : "disable")} the map '{map.Name}'", actor);
            return false;
        }

        await chat.SuccessMessageAsync($"'{map.Name}' was {(enable ? "enabled" : "disabled")}.", actor);
        return true;
    }

    private static bool Contains(string? text, string search) =>
        text != null && FormattingUtils.CleanTmFormatting(text).Contains(search, StringComparison.OrdinalIgnoreCase);

    private static IEnumerable<IMapListMap> Sort(IEnumerable<IMapListMap> maps, MapListFilter filter)
    {
        // maps without a pb are always placed last when sorting by pb
        Func<IMapListMap, object> key = filter.Sort switch
        {
            MapListSort.Author => m => FormattingUtils.CleanTmFormatting(m.Map.Author?.NickName ?? "").ToLowerInvariant(),
            MapListSort.Pb => m => m.Pb?.Score ?? int.MaxValue,
            MapListSort.Favorite => m => m.IsFavorite ? 0 : 1,
            _ => m => FormattingUtils.CleanTmFormatting(m.Map.Name).ToLowerInvariant()
        };

        var ordered = filter.Descending ? maps.OrderByDescending(key) : maps.OrderBy(key);
        return ordered.ThenBy(m => FormattingUtils.CleanTmFormatting(m.Map.Name).ToLowerInvariant());
    }

    public async Task<bool> SetFavoriteAsync(IPlayer player, string mapUid, bool favorite)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);

        if (map == null)
        {
            return false;
        }

        await repository.SetFavoriteAsync(player.Id, map.Id, favorite);
        return true;
    }

    public async Task<bool> AddTagAsync(string mapUid, string tag)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        tag = tag.Trim();

        if (map == null || tag.Length is 0 or > 32)
        {
            return false;
        }

        await repository.AddTagToMapAsync(map.Id, tag);
        return true;
    }

    public async Task<bool> RemoveTagAsync(string mapUid, string tag)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);
        return map != null && await repository.RemoveTagFromMapAsync(map.Id, tag.Trim());
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

            await chat.ErrorMessageAsync($"Failed to remove the map '{map.Name}'", actor);
            return;
        }
        
        await chat.SuccessMessageAsync($"'{map.Name}' was removed from the map list.", actor);
    }

    public Task OpenMapListAsync(IPlayer player)
    {
        state.ResetFilter(player);
        return ShowMapListAsync(player);
    }

    public async Task ShowMapListAsync(IPlayer player)
    {
        var filter = state.GetFilter(player);
        var canRemoveMaps = await permissions.HasPermissionAsync(player, MapsPermissions.RemoveMap);
        var canToggleMaps = await permissions.HasPermissionAsync(player, MapListPermissions.ToggleMapEnabled);
        var result = await GetFilteredMapsForPlayerAsync(player, filter);

        await manialinkManager.SendManialinkAsync(player, "MapListModule.MapList",
            new
            {
                result,
                filter,
                canRemoveMaps,
                canToggleMaps,
                canAddMaps = await permissions.HasPermissionAsync(player, MapsPermissions.AddMap),
                canManageTags = await permissions.HasPermissionAsync(player, MapListPermissions.ManageTags)
            });
    }

    public Task ShowTagsDialogAsync(IPlayer player, string mapUid) => SendTagsDialogAsync(player, mapUid, false);

    public Task ShowTagsViewDialogAsync(IPlayer player, string mapUid) => SendTagsDialogAsync(player, mapUid, true);

    private async Task SendTagsDialogAsync(IPlayer player, string mapUid, bool readOnly)
    {
        var map = await mapService.GetMapByUidAsync(mapUid);

        if (map == null)
        {
            return;
        }

        var tags = await repository.GetTagsByMapAsync();

        await manialinkManager.SendManialinkAsync(player, "MapListModule.Dialogs.MapTagsDialog", new
        {
            mapName = map.Name,
            readOnly,
            tags = (IEnumerable<IMapTag>)(tags.TryGetValue(map.Id, out var mapTags) ? mapTags : [])
        });
    }

    public async Task ShowAddMapDialogAsync(IPlayer player)
    {
        var tmx = state.GetTmxState(player);

        await manialinkManager.SendManialinkAsync(player, "MapListModule.Dialogs.AddMapDialog", new
        {
            tmx,
            selected = tmx.Result.Maps.FirstOrDefault(m => m.Id == tmx.SelectedId),
            pageSize = ITmxSearchService.PageSize
        });
    }

    public Task ConfirmMapDeletionAsync(IPlayer player, IMap map) => 
        manialinkManager.SendManialinkAsync(player, "MapListModule.Dialogs.ConfirmDeleteDialog", new
        {
            mapName = map.Name,
            mapUid = map.Uid
        });
}
