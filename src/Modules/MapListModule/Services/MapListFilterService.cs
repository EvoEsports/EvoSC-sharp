using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MapListFilterService(IMapListStateService state) : IMapListFilterService
{
    public void SetSearch(IPlayer player, string? search) => state.GetFilter(player).Search = search ?? "";

    public void SetFilter(IPlayer player, string name, bool enabled)
    {
        var filter = state.GetFilter(player);

        switch (name)
        {
            case "favorites":
                filter.FavoritesOnly = enabled;
                break;
            case "disabled":
                filter.DisabledOnly = enabled;
                break;
            case "nofinish":
                filter.NoFinishOnly = enabled;
                break;
        }
    }

    public void SetTag(IPlayer player, string tag, bool active)
    {
        var filter = state.GetFilter(player);
        filter.Tags.RemoveAll(t => string.Equals(t, tag, StringComparison.OrdinalIgnoreCase));

        if (active)
        {
            filter.Tags.Add(tag);
        }
    }

    public void SetAuthor(IPlayer player, string accountId, bool active)
    {
        var filter = state.GetFilter(player);
        filter.AuthorAccountId = active ? accountId : null;
        filter.AuthorName = null;
    }

    public bool SetSort(IPlayer player, string column, bool descending)
    {
        if (!Enum.TryParse<MapListSort>(column, true, out var sort))
        {
            return false;
        }

        var filter = state.GetFilter(player);
        filter.Sort = sort;
        filter.Descending = descending;
        return true;
    }
}
