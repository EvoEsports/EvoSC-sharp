using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

/// <summary>
/// Handles searching Trackmania Exchange and adding maps from it, for the "add map" dialog.
/// </summary>
public interface IMapListTmxService
{
    /// <summary>
    /// Load the first page of results if the player has not searched yet.
    /// </summary>
    public Task EnsureLoadedAsync(IPlayer player);

    /// <summary>
    /// Search for maps by name and go to the first page.
    /// </summary>
    public Task SearchAsync(IPlayer player, string query);

    /// <summary>
    /// Go to a page of the current search.
    /// </summary>
    public Task GoToPageAsync(IPlayer player, int page);

    /// <summary>
    /// Select a map in the results to show its details.
    /// </summary>
    public void Select(IPlayer player, int id);

    /// <summary>
    /// Download a map from Trackmania Exchange, add it and carry over its tags. The outcome is
    /// stored as a message in the player's state. Unexpected errors are rethrown after the message is set.
    /// </summary>
    /// <param name="player">Player that adds the map.</param>
    /// <param name="id">Trackmania Exchange ID of the map.</param>
    /// <returns>The UID of the added map, or null if the map was not added.</returns>
    public Task<string?> AddMapAsync(IPlayer player, int id);
}
