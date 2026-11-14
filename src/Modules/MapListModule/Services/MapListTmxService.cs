using System.Data;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Models;
using EvoSC.Modules.Official.MapsModule.Interfaces;

namespace EvoSC.Modules.Official.MapListModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MapListTmxService(
    IMapService mapService,
    IMxMapService mxMapService,
    IMapListService mapListService,
    IMapListStateService state,
    ITmxSearchService tmxSearch) : IMapListTmxService
{
    public async Task EnsureLoadedAsync(IPlayer player)
    {
        var tmx = state.GetTmxState(player);

        if (tmx.Result.Maps.Count == 0)
        {
            await LoadPageAsync(tmx, tmx.Query, 0);
        }
    }

    public Task SearchAsync(IPlayer player, string query) =>
        LoadPageAsync(state.GetTmxState(player), query, 0);

    public Task GoToPageAsync(IPlayer player, int page)
    {
        var tmx = state.GetTmxState(player);
        return LoadPageAsync(tmx, tmx.Query, Math.Max(page, 0));
    }

    public void Select(IPlayer player, int id)
    {
        var tmx = state.GetTmxState(player);
        tmx.SelectedId = id;
        tmx.Message = null;
    }

    public async Task<string?> AddMapAsync(IPlayer player, int id)
    {
        var tmx = state.GetTmxState(player);

        try
        {
            var tmxInfo = tmx.Result.Maps.FirstOrDefault(m => m.Id == id);
            var existing = await mapService.GetMapByExternalIdAsync(id.ToString()) ??
                           (string.IsNullOrEmpty(tmxInfo?.Uid) ? null : await mapService.GetMapByUidAsync(tmxInfo.Uid));

            if (existing != null)
            {
                tmx.Message = "Map already in map list.";
                return null;
            }

            var map = await mxMapService.FindAndDownloadMapAsync(id, null, player);

            if (map == null)
            {
                tmx.Message = "Could not download the map from Trackmania Exchange.";
                return null;
            }

            // carry over the Trackmania Exchange tags as map list tags
            foreach (var tag in tmxInfo?.Tags ?? [])
            {
                await mapListService.AddTagAsync(map.Uid, tag);
            }

            tmx.Message = $"Added '{map.Name}'.";
            return map.Uid;
        }
        catch (DuplicateNameException)
        {
            tmx.Message = "That map has already been added.";
            return null;
        }
        catch (FileLoadException)
        {
            tmx.Message = "The server can't find the downloaded map file. Check the maps path in the EvoSC config.";
            throw;
        }
        catch (Exception)
        {
            tmx.Message = "Failed to add the map.";
            throw;
        }
    }

    private async Task LoadPageAsync(TmxSearchState tmx, string query, int page)
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
