using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Models;

namespace EvoSC.Modules.Official.MapListModule.Interfaces;

/// <summary>
/// Keeps track of the search, filter and sort options each player has chosen in the map list.
/// </summary>
public interface IMapListStateService
{
    /// <summary>
    /// Get the current filter of a player. A default filter is created if none exists.
    /// </summary>
    public MapListFilter GetFilter(IPlayer player);

    /// <summary>
    /// Reset a player's filter to the defaults.
    /// </summary>
    public void ResetFilter(IPlayer player);

    /// <summary>
    /// Get the UID of the map whose tags the player is currently editing, if any.
    /// </summary>
    public string? GetTagMapUid(IPlayer player);

    /// <summary>
    /// Set the UID of the map whose tags the player is editing. Null if the player is done.
    /// </summary>
    public void SetTagMapUid(IPlayer player, string? mapUid);

    /// <summary>
    /// Get the state of the player's "add map" dialog. A default state is created if none exists.
    /// </summary>
    public TmxSearchState GetTmxState(IPlayer player);
}
