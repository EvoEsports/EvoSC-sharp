using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

/// <summary>
/// Changes the search, filter and sort options a player has chosen in the map list.
/// </summary>
public interface IMapListFilterService
{
    /// <summary>
    /// Set the search text. Null or empty clears the search.
    /// </summary>
    public void SetSearch(IPlayer player, string? search);

    /// <summary>
    /// Turn one of the filters (favorites, nofinish or disabled) on or off.
    /// </summary>
    public void SetFilter(IPlayer player, string name, bool enabled);

    /// <summary>
    /// Add a tag to the tag filter or remove it.
    /// </summary>
    public void SetTag(IPlayer player, string tag, bool active);

    /// <summary>
    /// Filter by an author's maps, or clear the author filter.
    /// </summary>
    public void SetAuthor(IPlayer player, string accountId, bool active);

    /// <summary>
    /// Sort by a column in the given direction.
    /// </summary>
    /// <returns>False if the column is not known.</returns>
    public bool SetSort(IPlayer player, string column, bool descending);
}
