using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Mount;

/// <summary>
/// One (Kind, Interactive) bucket's reserved, contiguous range of local slot indices within its
/// Kind's array -- e.g. Quad/non-interactive might own local indices [0, 40) and
/// Quad/interactive [40, 55). Non-interactive always comes before interactive for a given Kind.
/// </summary>
public readonly record struct PoolBucketRange(VKind Kind, bool Interactive, int Start, int Count);

/// <summary>
/// Computes a deterministic slot layout from a pool budget: the single source of truth both
/// <see cref="SlotAllocator"/> (which slot number a fresh Alloc hands out) and
/// <c>MountXmlWriter</c> (which physical control gets <c>scriptevents="1"</c>, and where) must
/// agree on. They agree by both calling this rather than each computing their own ordering --
/// see the note on why this matters: the mount XML is generated once, upfront, before any
/// rendering happens, so which local index is interactive cannot depend on runtime allocation
/// order.
/// </summary>
public static class PoolLayout
{
    public static IReadOnlyList<PoolBucketRange> Compute(
        IReadOnlyDictionary<(VKind Kind, bool Interactive), int> capacityByBucket)
    {
        var ranges = new List<PoolBucketRange>();
        var nextStartByKind = new Dictionary<VKind, int>();

        foreach (var kind in Enum.GetValues<VKind>())
        {
            // Non-interactive before interactive: an arbitrary but fixed choice, since all that
            // matters is that every consumer of this layout agrees on the same one.
            foreach (var interactive in new[] { false, true })
            {
                if (!capacityByBucket.TryGetValue((kind, interactive), out var capacity) || capacity == 0)
                {
                    continue;
                }

                var start = nextStartByKind.GetValueOrDefault(kind);
                ranges.Add(new PoolBucketRange(kind, interactive, start, capacity));
                nextStartByKind[kind] = start + capacity;
            }
        }

        return ranges;
    }
}
