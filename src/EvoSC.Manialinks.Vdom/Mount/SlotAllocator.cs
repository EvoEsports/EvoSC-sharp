using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Mount;

/// <summary>
/// Fixed-capacity <see cref="ISlotAllocator"/>. Built from the same <see cref="PoolLayout"/>
/// <c>MountXmlWriter</c> uses, so a fresh Alloc for (Kind, Interactive) always lands within the
/// exact local-index range that was baked into the mount XML for that bucket -- never a slot
/// index that mount time gave to a different bucket of the same Kind.
/// </summary>
public sealed class SlotAllocator : ISlotAllocator
{
    private readonly Dictionary<(VKind Kind, bool Interactive), Bucket> _buckets;

    public SlotAllocator(IReadOnlyDictionary<(VKind Kind, bool Interactive), int> capacityByBucket)
        : this(PoolLayout.Compute(capacityByBucket))
    {
    }

    public SlotAllocator(IReadOnlyList<PoolBucketRange> layout)
    {
        _buckets = layout.ToDictionary(r => (r.Kind, r.Interactive), r => new Bucket(r.Start, r.Count));
    }

    public int Alloc(VKind kind, bool interactive)
    {
        var bucket = GetBucketOrThrow(kind, interactive);

        if (bucket.TryReuse(out var reused))
        {
            return reused;
        }

        if (bucket.TryFresh(out var fresh))
        {
            return fresh;
        }

        throw new SlotPoolExhaustedException(kind, interactive);
    }

    public void Free(int slot, VKind kind, bool interactive) => GetBucketOrThrow(kind, interactive).Free(slot);

    private Bucket GetBucketOrThrow(VKind kind, bool interactive) =>
        _buckets.TryGetValue((kind, interactive), out var bucket)
            ? bucket
            : throw new SlotPoolExhaustedException(kind, interactive);

    private sealed class Bucket(int rangeStart, int capacity)
    {
        private readonly int _rangeEnd = rangeStart + capacity;
        private readonly Stack<int> _free = new();
        private int _nextFresh = rangeStart;

        public bool TryReuse(out int slot) => _free.TryPop(out slot);

        public bool TryFresh(out int slot)
        {
            if (_nextFresh >= _rangeEnd)
            {
                slot = default;
                return false;
            }

            slot = _nextFresh++;
            return true;
        }

        public void Free(int slot) => _free.Push(slot);
    }
}
