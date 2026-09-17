using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Views;

/// <summary>
/// What a module implements to describe a reactive widget: a name, a fixed pool budget, and a
/// pure function from props to a <see cref="VNode"/> tree. See <c>Builder/</c> for the typed
/// helpers used to build that tree.
/// </summary>
public interface IVdomView
{
    /// <summary>
    /// Identifies this view's mount/patch pages and LocalUser variable (see
    /// <see cref="VdomNaming"/>) -- must be unique across every mounted view.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// How many controls of each (Kind, Interactive) bucket this view's pool needs -- see the
    /// plan's M0 findings on why interactive and non-interactive need separate budgets. Fixed
    /// for the view's lifetime; exceeding it triggers a remount (see
    /// <see cref="ISlotAllocator"/>).
    /// </summary>
    IReadOnlyDictionary<(VKind Kind, bool Interactive), int> PoolBudget { get; }

    /// <summary>
    /// Builds the full tree for the given props. Must be pure and total -- called on every
    /// mount and every update, and the <see cref="Reconciler"/> assumes each call reflects the
    /// complete current state, not an incremental change.
    /// </summary>
    VNode Render(object props);
}
