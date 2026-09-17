namespace EvoSC.Manialinks.Vdom.Vdom;

/// <summary>
/// A virtual node: the author-facing description of one control (or, for
/// <see cref="VKind.Frame"/> with <see cref="VContainerNeed.None"/>, a pure grouping/transform
/// node with no control of its own -- see <c>Mount/LayoutFlattener.cs</c>).
///
/// <paramref name="Key"/> identifies this node's identity across renders for keyed
/// reconciliation of its position among siblings; nodes without a key are matched positionally.
/// It does not need to be unique across the whole tree, only among a node's own siblings.
///
/// Every render call is expected to build a fresh tree with the *complete* current state (same
/// discipline as React/Vue), not to mutate a previous one -- the <see cref="Reconciler"/> is what
/// turns two full trees into the minimal set of changes.
/// </summary>
public sealed record VNode(
    VKind Kind,
    object? Key,
    VProps Props,
    VMountProps MountProps,
    IReadOnlyList<VNode> Children,
    VContainerNeed Container = VContainerNeed.None)
{
    /// <summary>A leaf control: Quad, Label, or Entry.</summary>
    public static VNode Leaf(VKind kind, object? key, VProps props, VMountProps? mountProps = null)
    {
        if (kind == VKind.Frame)
        {
            throw new ArgumentException(
                $"{nameof(Leaf)} is for Quad/Label/Entry; use {nameof(Group)} for a Frame.",
                nameof(kind));
        }

        return new VNode(kind, key, props, mountProps ?? VMountProps.Default, Array.Empty<VNode>());
    }

    /// <summary>
    /// A pure grouping/transform node: contributes <paramref name="props"/>' Position /
    /// RelativeScale / RelativeRotation to its descendants during layout flattening but is
    /// itself erased (consumes no pool slot) unless <paramref name="container"/> is set.
    /// </summary>
    public static VNode Group(
        object? key,
        IReadOnlyList<VNode> children,
        VProps? props = null,
        VContainerNeed container = VContainerNeed.None) =>
        new(VKind.Frame, key, props ?? VProps.Empty, VMountProps.Default, children, container);
}
