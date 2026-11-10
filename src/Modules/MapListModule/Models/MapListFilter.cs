using EvoSC.Modules.Official.MapListModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MapListModule.Models;

public enum MapListSort
{
    Name,
    Author,
    Pb,
    Favorite
}

/// <summary>
/// Search, filter and sort options of a player's map list.
/// </summary>
public class MapListFilter
{
    /// <summary>
    /// Text to search for in map names and author names.
    /// </summary>
    public string Search { get; set; } = "";

    public bool FavoritesOnly { get; set; }

    /// <summary>
    /// Only show maps the player has not finished.
    /// </summary>
    public bool NoFinishOnly { get; set; }

    /// <summary>
    /// Only show disabled maps. Only has an effect for players that can see disabled maps.
    /// </summary>
    public bool DisabledOnly { get; set; }

    /// <summary>
    /// Only show maps that have all of these tags.
    /// </summary>
    public List<string> Tags { get; set; } = [];

    /// <summary>
    /// Only show maps by the author with this account ID.
    /// </summary>
    public string? AuthorAccountId { get; set; }

    /// <summary>
    /// Display name of the author that is filtered on.
    /// </summary>
    public string? AuthorName { get; set; }

    public MapListSort Sort { get; set; } = MapListSort.Name;

    public bool Descending { get; set; }
}

/// <summary>
/// The result of applying a filter to the map list.
/// </summary>
public record MapListResult(IReadOnlyList<IMapListMap> Maps, int TotalCount, IReadOnlyList<IMapTag> AllTags);
