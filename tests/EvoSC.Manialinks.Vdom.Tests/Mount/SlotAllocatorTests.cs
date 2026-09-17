using EvoSC.Manialinks.Vdom.Mount;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Mount;

public class SlotAllocatorTests
{
    private static SlotAllocator NewAllocator(int quadCapacity = 2, int labelCapacity = 2) =>
        new(new Dictionary<(VKind, bool), int>
        {
            [(VKind.Quad, false)] = quadCapacity,
            [(VKind.Label, false)] = labelCapacity
        });

    [Fact]
    public void Alloc_hands_out_sequential_slots_within_capacity()
    {
        var allocator = NewAllocator(quadCapacity: 3);

        var a = allocator.Alloc(VKind.Quad, false);
        var b = allocator.Alloc(VKind.Quad, false);
        var c = allocator.Alloc(VKind.Quad, false);

        Assert.Equal(new[] { 0, 1, 2 }, new[] { a, b, c });
    }

    [Fact]
    public void Alloc_throws_once_a_buckets_capacity_is_exhausted()
    {
        var allocator = NewAllocator(quadCapacity: 1);
        allocator.Alloc(VKind.Quad, false);

        var ex = Assert.Throws<SlotPoolExhaustedException>(() => allocator.Alloc(VKind.Quad, false));

        Assert.Equal(VKind.Quad, ex.Kind);
        Assert.False(ex.Interactive);
    }

    [Fact]
    public void Alloc_for_an_undeclared_bucket_throws_immediately()
    {
        var allocator = NewAllocator();

        // (Quad, interactive: true) was never given a capacity.
        Assert.Throws<SlotPoolExhaustedException>(() => allocator.Alloc(VKind.Quad, true));
    }

    [Fact]
    public void Freeing_a_slot_lets_the_next_alloc_reuse_it()
    {
        var allocator = NewAllocator(quadCapacity: 1);
        var slot = allocator.Alloc(VKind.Quad, false);

        allocator.Free(slot, VKind.Quad, false);
        var reused = allocator.Alloc(VKind.Quad, false);

        Assert.Equal(slot, reused);
    }

    [Fact]
    public void Exhausting_one_bucket_does_not_affect_another()
    {
        var allocator = NewAllocator(quadCapacity: 1, labelCapacity: 1);
        allocator.Alloc(VKind.Quad, false);

        // Should not throw -- the Label bucket has its own, untouched capacity and counter.
        var labelSlot = allocator.Alloc(VKind.Label, false);

        Assert.Equal(0, labelSlot);
    }

    [Fact]
    public void Slot_numbers_are_scoped_per_kind_not_global()
    {
        var allocator = NewAllocator();

        // The client indexes into one array per Kind (see VPatchOp), so a Quad slot and a Label
        // slot may validly share the same number -- they are never compared to each other.
        var quadSlot = allocator.Alloc(VKind.Quad, false);
        var labelSlot = allocator.Alloc(VKind.Label, false);

        Assert.Equal(0, quadSlot);
        Assert.Equal(0, labelSlot);
    }

    [Fact]
    public void Interactive_and_non_interactive_slots_of_the_same_kind_share_one_counter()
    {
        var allocator = new SlotAllocator(new Dictionary<(VKind, bool), int>
        {
            [(VKind.Quad, false)] = 5,
            [(VKind.Quad, true)] = 5
        });

        var plain = allocator.Alloc(VKind.Quad, false);
        var interactive = allocator.Alloc(VKind.Quad, true);

        // Both come from the same per-(Quad) counter, so they must not collide even though they
        // live in separate free-list buckets.
        Assert.NotEqual(plain, interactive);
    }
}
