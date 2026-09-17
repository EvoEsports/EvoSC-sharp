namespace EvoSC.Manialinks.Vdom.Vdom;

/// <summary>
/// Hands out and reclaims pool slot numbers for a mounted view, scoped per
/// <c>(VKind, Interactive)</c> -- see the plan's M0 findings on why interactive and
/// non-interactive controls need separate budgets even at the same <see cref="VKind"/>. The real
/// implementation (<c>Mount.SlotAllocator</c>) is fixed-capacity, sized from the mount-time pool
/// plan; capacity is defined here at the interface only in spirit -- see
/// <see cref="SlotPoolExhaustedException"/>.
/// </summary>
public interface ISlotAllocator
{
    /// <summary>
    /// Claims a slot for <paramref name="kind"/> / <paramref name="interactive"/>, preferring a
    /// freed slot over a never-used one. Throws <see cref="SlotPoolExhaustedException"/> if none
    /// remain -- the designed escape hatch for that is a full remount, not a caught-and-ignored
    /// error, so callers should let it propagate to whatever triggers a remount.
    /// </summary>
    int Alloc(VKind kind, bool interactive);

    /// <summary>Returns a slot to the free list for reuse by a later Alloc of the same bucket.</summary>
    void Free(int slot, VKind kind, bool interactive);
}

/// <summary>
/// Thrown by <see cref="ISlotAllocator.Alloc"/> when a <c>(VKind, Interactive)</c> pool has no
/// slots left. Not a bug to catch and paper over -- the caller is expected to fall back to a
/// full remount with a larger pool budget.
/// </summary>
public sealed class SlotPoolExhaustedException(VKind kind, bool interactive)
    : Exception($"Slot pool exhausted for {kind} (interactive: {interactive}).")
{
    public VKind Kind { get; } = kind;
    public bool Interactive { get; } = interactive;
}
