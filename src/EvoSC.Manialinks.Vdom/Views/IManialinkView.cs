using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Manialinks.Vdom.Views;

/// <summary>A view mounted for a specific audience of players.</summary>
public interface IManialinkView
{
    string Name { get; }

    /// <summary>
    /// Re-renders with new props, diffs against the current mount state, and sends a patch to
    /// the audience if anything changed. A no-op diff sends nothing.
    /// </summary>
    Task UpdateAsync(object props);

    /// <summary>
    /// Sends a fresh mount (reflecting current state, not the original initial render) to
    /// players not already in this view's audience -- the reactive-view equivalent of
    /// <c>IManialinkManager</c>'s existing behaviour of re-sending persistent manialinks to
    /// newly-connected players. Players already in the audience are unaffected.
    /// </summary>
    Task AddPlayersAsync(IEnumerable<IPlayer> players);

    /// <summary>Hides the view for its whole audience and releases its tracked state.</summary>
    Task UnmountAsync();
}
