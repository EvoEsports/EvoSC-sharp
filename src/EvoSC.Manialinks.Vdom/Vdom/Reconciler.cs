using EvoSC.Manialinks.Vdom.Mount;

namespace EvoSC.Manialinks.Vdom.Vdom;

/// <summary>
/// Thrown when a <see cref="VMountProps"/> value differs between an old and new node that
/// otherwise matched by identity and <see cref="VKind"/>. Mount-only props (class, scriptevents)
/// cannot be changed on an already-allocated slot -- see the plan: "Binding a mount-only prop to
/// changing data must be a mount-time error, not a silent no-op." If the value is genuinely
/// meant to vary, give the node a key that changes with it, which forces a Free+Alloc instead.
/// </summary>
public sealed class MountOnlyPropsChangedException(string identity, VMountProps oldProps, VMountProps newProps)
    : InvalidOperationException(
        $"Node '{identity}' changed its mount-only props (Class/ScriptEvents) between renders "
        + $"without changing key or kind: was {{ Class: {oldProps.Class}, ScriptEvents: {oldProps.ScriptEvents} }}, "
        + $"now {{ Class: {newProps.Class}, ScriptEvents: {newProps.ScriptEvents} }}. Give this node a key that "
        + "varies with whatever determines these values, so the change becomes a Free+Alloc instead.")
{
    public string Identity { get; } = identity;
    public VMountProps OldProps { get; } = oldProps;
    public VMountProps NewProps { get; } = newProps;
}

/// <summary>
/// Thrown when the new tree assigns the same identity to two sibling nodes -- almost always an
/// authoring bug (e.g. a foreach using an index-independent key that collides), and one that
/// would otherwise silently double-book a single pool slot.
/// </summary>
public sealed class DuplicateNodeIdentityException(string identity)
    : InvalidOperationException(
        $"Two nodes in the same render both resolved to identity '{identity}'. Give them distinct " +
        "keys (or check a foreach's key selector for collisions).")
{
    public string Identity { get; } = identity;
}

/// <summary>
/// Diffs a previously-mounted set of leaves against a freshly-rendered <see cref="VNode"/> tree,
/// producing the minimal ordered list of <see cref="VPatchOp"/>s to bring the mount up to date.
///
/// Layer boundary note: the plan lists this as L2 and layout flattening as L3, but functionally
/// this operates on <see cref="LayoutFlattener"/>'s output (L3) rather than the raw nested tree
/// -- diffing already-flattened, absolute-position leaves is a plain keyed-list problem, and
/// keeps <see cref="LayoutFlattener"/> (transform composition) independently testable without
/// involving reconciliation at all. See the class docs there for why.
/// </summary>
public static class Reconciler
{
    /// <param name="previouslyMounted">
    /// The mount state returned by the previous call (empty for an initial mount -- every leaf
    /// is then a fresh Alloc).
    /// </param>
    /// <param name="newTree">The freshly-built tree for this render.</param>
    /// <param name="allocator">
    /// Slot source for newly-needed leaves; also receives the corresponding Free calls so its
    /// free lists stay in sync with what <paramref name="previouslyMounted"/> will be replaced
    /// with. See <see cref="ISlotAllocator"/> for why it is keyed by (Kind, Interactive).
    /// </param>
    /// <returns>
    /// The ops to send (in order: matched-node Sets, new-node Alloc+Sets, then Frees for
    /// anything no longer present) and the mount state to pass into the next call.
    /// </returns>
    public static (IReadOnlyList<VPatchOp> Ops, IReadOnlyList<MountedNode> Mounted) Diff(
        IReadOnlyList<MountedNode> previouslyMounted,
        VNode newTree,
        ISlotAllocator allocator)
    {
        var newFlat = LayoutFlattener.Flatten(newTree);
        var oldByIdentity = new Dictionary<string, MountedNode>(previouslyMounted.Count);

        foreach (var old in previouslyMounted)
        {
            oldByIdentity.Add(old.Flat.Identity, old);
        }

        var ops = new List<VPatchOp>();
        var newMounted = new List<MountedNode>(newFlat.Count);
        var matchedOldIdentities = new HashSet<string>();
        var seenNewIdentities = new HashSet<string>();

        MountedNode AllocNew(FlatNode flat)
        {
            var slot = allocator.Alloc(flat.Kind, flat.MountProps.ScriptEvents);
            ops.Add(new VPatchOp.Alloc(slot, flat.Kind, flat.MountProps.ScriptEvents));

            foreach (var change in flat.Props.AllSet())
            {
                ops.Add(new VPatchOp.Set(slot, flat.Kind, change.Id, change.Value));
            }

            return new MountedNode(flat, slot);
        }

        void FreeOld(MountedNode old)
        {
            ops.Add(new VPatchOp.Free(old.Slot, old.Flat.Kind, old.Flat.MountProps.ScriptEvents));
            allocator.Free(old.Slot, old.Flat.Kind, old.Flat.MountProps.ScriptEvents);
        }

        foreach (var flat in newFlat)
        {
            if (!seenNewIdentities.Add(flat.Identity))
            {
                throw new DuplicateNodeIdentityException(flat.Identity);
            }

            if (!oldByIdentity.TryGetValue(flat.Identity, out var old))
            {
                newMounted.Add(AllocNew(flat));
                continue;
            }

            matchedOldIdentities.Add(flat.Identity);

            if (old.Flat.Kind != flat.Kind)
            {
                // A kind swap invalidates the slot outright: props don't line up across kinds
                // (a Label's Text has nowhere to go on a Quad), so this is a Free+Alloc, not a
                // prop diff.
                FreeOld(old);
                newMounted.Add(AllocNew(flat));
                continue;
            }

            if (old.Flat.MountProps != flat.MountProps)
            {
                throw new MountOnlyPropsChangedException(flat.Identity, old.Flat.MountProps, flat.MountProps);
            }

            foreach (var change in flat.Props.DiffFrom(old.Flat.Props))
            {
                ops.Add(new VPatchOp.Set(old.Slot, flat.Kind, change.Id, change.Value));
            }

            newMounted.Add(new MountedNode(flat, old.Slot));
        }

        foreach (var old in previouslyMounted)
        {
            if (!matchedOldIdentities.Contains(old.Flat.Identity))
            {
                FreeOld(old);
            }
        }

        return (ops, newMounted);
    }
}
