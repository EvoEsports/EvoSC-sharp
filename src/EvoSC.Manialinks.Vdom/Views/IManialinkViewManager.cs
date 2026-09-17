using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Manialinks.Vdom.Views;

public interface IManialinkViewManager
{
    /// <summary>Mounts <paramref name="view"/> once for <paramref name="players"/> and sends the initial render.</summary>
    Task<IManialinkView> MountAsync(IEnumerable<IPlayer> players, IVdomView view, object props);

    /// <summary>
    /// Records that <paramref name="login"/> has applied everything through <paramref name="seq"/>
    /// for the named view -- called by <c>VdomAckController</c> when the client's
    /// <c>TriggerPageAction</c> ack arrives. A no-op if the view is unknown (e.g. already
    /// unmounted) or the player isn't in its audience.
    /// </summary>
    void RecordAck(string viewName, string login, int seq);
}
