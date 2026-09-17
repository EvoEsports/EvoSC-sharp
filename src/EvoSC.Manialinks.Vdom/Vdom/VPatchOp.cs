namespace EvoSC.Manialinks.Vdom.Vdom;

/// <summary>
/// One change produced by <see cref="Reconciler.Diff"/>. <c>Slot</c> is unique *within its
/// <see cref="VKind"/>* -- the client keeps one flat array per Kind (populated once at mount via
/// <c>Page.GetClassChildren</c>, see the plan's L5), and every op carries its own Kind so a Set
/// is dispatchable on its own without the client tracking anything about prior Alloc/Free
/// traffic. <c>Interactive</c> only matters server-side, for which (Kind, Interactive) free list
/// an Alloc/Free draws from or returns to -- see the plan's M0 findings on why those need
/// separate budgets. A slot is never a game-side control id.
/// </summary>
public abstract record VPatchOp
{
    private VPatchOp()
    {
    }

    /// <summary>
    /// A previously-free slot has been claimed for a new node. Always followed, in the same
    /// diff, by <see cref="Set"/> ops for every prop the new node declares (see
    /// <see cref="VProps.AllSet"/>) -- <c>Alloc</c> itself carries no prop values.
    /// </summary>
    public sealed record Alloc(int Slot, VKind Kind, bool Interactive) : VPatchOp;

    /// <summary>
    /// A slot is no longer in use and has been returned to its (Kind, Interactive) free list.
    /// The control physically still exists (controls can't be destroyed -- see the plan's "Hard
    /// walls") and keeps whatever it last displayed until the slot is reallocated; hiding it is
    /// the transport/runtime's job (e.g. via a <see cref="VPropId.Visible"/> Set), not this op's.
    /// </summary>
    public sealed record Free(int Slot, VKind Kind, bool Interactive) : VPatchOp;

    /// <summary>One prop write on an already-allocated slot.</summary>
    public sealed record Set(int Slot, VKind Kind, VPropId Prop, object Value) : VPatchOp;
}
