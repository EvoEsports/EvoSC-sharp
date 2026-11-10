using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

public interface ITmxSearchService
{
    /// <summary>
    /// The number of maps on each page of results.
    /// </summary>
    public const int PageSize = 7;

    /// <summary>
    /// Search Trackmania Exchange for maps by name. An empty query returns the newest maps.
    /// </summary>
    /// <param name="query">Text to search for in map names.</param>
    /// <param name="page">Zero-based page number.</param>
    /// <returns>The results, or null if Trackmania Exchange could not be reached.</returns>
    public Task<TmxSearchResult?> SearchAsync(string query, int page);
}
