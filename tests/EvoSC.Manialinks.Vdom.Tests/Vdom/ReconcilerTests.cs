using EvoSC.Manialinks.Vdom.Mount;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Vdom;

public class ReconcilerTests
{
    // A minimal LiveRanking-shaped tree: a list of keyed rows, each a Frame grouping one Quad
    // (background) and one Label (name), positioned by row index -- mirrors
    // LiveRankingRecordRow.mt's `pos="0 {{ y }}"` / `y="{{ -__index*(4+0.3) }}"` shape closely
    // enough to exercise the same reconciliation patterns without depending on the game.
    private static VNode Row(object key, double y, string name) =>
        VNode.Group(
            key,
            new[]
            {
                VNode.Leaf(VKind.Quad, null, new VProps { BgColor = VColor.FromHex("0AF") }),
                VNode.Leaf(VKind.Label, null, new VProps { Text = name })
            },
            props: new VProps { Position = new VVec2(0, y) });

    private static VNode Root(params VNode[] rows) => VNode.Group(null, rows);

    private static SlotAllocator NewAllocator() => new(new Dictionary<(VKind, bool), int>
    {
        [(VKind.Quad, false)] = 100,
        [(VKind.Quad, true)] = 100,
        [(VKind.Label, false)] = 100,
        [(VKind.Label, true)] = 100
    });

    private static IReadOnlyList<T> OfType<T>(IReadOnlyList<VPatchOp> ops) where T : VPatchOp =>
        ops.OfType<T>().ToList();

    [Fact]
    public void Rendering_the_same_tree_twice_produces_no_ops()
    {
        var allocator = NewAllocator();
        var tree = Root(Row("A", 0, "Alice"), Row("B", -4.3, "Bob"));

        var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), tree, allocator);
        var (ops, _) = Reconciler.Diff(mounted, tree, allocator);

        Assert.Empty(ops);
    }

    [Fact]
    public void Initial_mount_allocates_and_sets_every_leaf()
    {
        var allocator = NewAllocator();
        var tree = Root(Row("A", 0, "Alice"), Row("B", -4.3, "Bob"));

        var (ops, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), tree, allocator);

        // 2 rows * 2 leaves (quad + label) each = 4 allocs.
        Assert.Equal(4, OfType<VPatchOp.Alloc>(ops).Count);
        Assert.Empty(OfType<VPatchOp.Free>(ops));
        Assert.Equal(4, mounted.Count);

        // Every mounted leaf's baked position matches its row.
        var positions = mounted.ToDictionary(m => m.Flat.Identity, m => m.Flat.Props.Position);
        Assert.Equal(new VVec2(0, 0), positions["A/0"]);
        Assert.Equal(new VVec2(0, 0), positions["A/1"]);
        Assert.Equal(new VVec2(0, -4.3), positions["B/0"]);
        Assert.Equal(new VVec2(0, -4.3), positions["B/1"]);
    }

    [Fact]
    public void Inserting_a_row_in_the_middle_only_allocs_the_new_row_and_repositions_what_shifted()
    {
        var allocator = NewAllocator();
        var oldTree = Root(Row("A", 0, "Alice"), Row("C", -4.3, "Carol"));
        var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), oldTree, allocator);

        // B is inserted between A and C; C's row now sits one slot further down.
        var newTree = Root(Row("A", 0, "Alice"), Row("B", -4.3, "Bob"), Row("C", -8.6, "Carol"));
        var (ops, mountedAfter) = Reconciler.Diff(mounted, newTree, allocator);

        // Only B's two leaves are newly allocated.
        var allocs = OfType<VPatchOp.Alloc>(ops);
        Assert.Equal(2, allocs.Count);
        Assert.Empty(OfType<VPatchOp.Free>(ops));

        // A is untouched: no Set ops reference A's slot at all.
        var aSlot = mountedAfter.Single(m => m.Flat.Identity == "A/0").Slot;
        Assert.DoesNotContain(OfType<VPatchOp.Set>(ops), op => op.Slot == aSlot);

        // C moved: both its leaves get a Position Set to the new Y, and keep their old slots
        // (matched by key, not re-allocated).
        var cQuadBefore = mounted.Single(m => m.Flat.Identity == "C/0");
        var cQuadAfter = mountedAfter.Single(m => m.Flat.Identity == "C/0");
        Assert.Equal(cQuadBefore.Slot, cQuadAfter.Slot);
        Assert.Contains(
            OfType<VPatchOp.Set>(ops),
            op => op.Slot == cQuadAfter.Slot && op.Prop == VPropId.Position && (VVec2)op.Value == new VVec2(0, -8.6));
    }

    [Fact]
    public void Removing_a_row_frees_only_its_own_leaves()
    {
        var allocator = NewAllocator();
        var oldTree = Root(Row("A", 0, "Alice"), Row("B", -4.3, "Bob"), Row("C", -8.6, "Carol"));
        var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), oldTree, allocator);
        var bQuadSlot = mounted.Single(m => m.Flat.Identity == "B/0").Slot;
        var bLabelSlot = mounted.Single(m => m.Flat.Identity == "B/1").Slot;

        var newTree = Root(Row("A", 0, "Alice"), Row("C", -4.3, "Carol"));
        var (ops, _) = Reconciler.Diff(mounted, newTree, allocator);

        var frees = OfType<VPatchOp.Free>(ops);
        Assert.Equal(2, frees.Count);
        Assert.Contains(frees, f => f.Slot == bQuadSlot && f.Kind == VKind.Quad);
        Assert.Contains(frees, f => f.Slot == bLabelSlot && f.Kind == VKind.Label);
        Assert.Empty(OfType<VPatchOp.Alloc>(ops));
    }

    [Fact]
    public void Reversing_a_three_row_list_matches_by_key_not_position_and_only_moves_what_actually_moved()
    {
        var allocator = NewAllocator();
        var oldTree = Root(Row("A", 0, "Alice"), Row("B", -4.3, "Bob"), Row("C", -8.6, "Carol"));
        var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), oldTree, allocator);

        var newTree = Root(Row("C", 0, "Carol"), Row("B", -4.3, "Bob"), Row("A", -8.6, "Alice"));
        var (ops, mountedAfter) = Reconciler.Diff(mounted, newTree, allocator);

        // A pure reorder: nothing is allocated or freed, every identity is matched by key.
        Assert.Empty(OfType<VPatchOp.Alloc>(ops));
        Assert.Empty(OfType<VPatchOp.Free>(ops));

        foreach (var identity in new[] { "A/0", "A/1", "B/0", "B/1", "C/0", "C/1" })
        {
            Assert.Equal(mounted.Single(m => m.Flat.Identity == identity).Slot,
                mountedAfter.Single(m => m.Flat.Identity == identity).Slot);
        }

        // The middle row (B) lands back on its own old position -- a full reversal of 3 leaves
        // it in place -- so it gets no Position Set at all.
        var bSlot = mountedAfter.Single(m => m.Flat.Identity == "B/0").Slot;
        Assert.DoesNotContain(OfType<VPatchOp.Set>(ops), op => op.Slot == bSlot && op.Prop == VPropId.Position);

        // A and C swapped ends, so both get repositioned.
        var aQuadSlot = mountedAfter.Single(m => m.Flat.Identity == "A/0").Slot;
        var cQuadSlot = mountedAfter.Single(m => m.Flat.Identity == "C/0").Slot;
        Assert.Contains(OfType<VPatchOp.Set>(ops),
            op => op.Slot == aQuadSlot && op.Prop == VPropId.Position && (VVec2)op.Value == new VVec2(0, -8.6));
        Assert.Contains(OfType<VPatchOp.Set>(ops),
            op => op.Slot == cQuadSlot && op.Prop == VPropId.Position && (VVec2)op.Value == new VVec2(0, 0));
    }

    [Fact]
    public void Changing_kind_at_the_same_key_frees_the_old_slot_and_allocates_a_fresh_one()
    {
        var allocator = NewAllocator();
        var oldTree = Root(VNode.Leaf(VKind.Quad, "x", new VProps { BgColor = VColor.FromHex("FFF") }));
        var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), oldTree, allocator);
        var oldSlot = mounted.Single().Slot;

        var newTree = Root(VNode.Leaf(VKind.Label, "x", new VProps { Text = "now a label" }));
        var (ops, mountedAfter) = Reconciler.Diff(mounted, newTree, allocator);
        var newSlot = mountedAfter.Single().Slot;

        // Free, then Alloc, then the fresh slot's initial Sets (Position is always among them --
        // see LayoutFlattener -- plus whatever the node itself declared).
        Assert.Equal(new VPatchOp.Free(oldSlot, VKind.Quad, false), ops[0]);
        Assert.Equal(new VPatchOp.Alloc(newSlot, VKind.Label, false), ops[1]);
        Assert.Contains(OfType<VPatchOp.Set>(ops), op => op.Slot == newSlot && op.Prop == VPropId.Position);
        Assert.Contains(
            new VPatchOp.Set(newSlot, VKind.Label, VPropId.Text, "now a label"),
            OfType<VPatchOp.Set>(ops));

        // The old Quad slot and the new Label slot may legitimately share a number -- they're
        // different per-Kind counters (see SlotAllocator) and are never compared to each other
        // in practice, since every op carries its own Kind. What actually matters is that they
        // are two *separate* allocations, which the Free-then-Alloc op sequence above already
        // establishes.
        Assert.Equal(VKind.Label, mountedAfter.Single().Flat.Kind);
    }

    [Fact]
    public void Changing_scriptevents_at_the_same_key_and_kind_is_a_mount_time_error()
    {
        var allocator = NewAllocator();
        var oldTree = Root(VNode.Leaf(VKind.Quad, "x", new VProps(), new VMountProps { ScriptEvents = false }));
        var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), oldTree, allocator);

        var newTree = Root(VNode.Leaf(VKind.Quad, "x", new VProps(), new VMountProps { ScriptEvents = true }));

        var ex = Assert.Throws<MountOnlyPropsChangedException>(() => Reconciler.Diff(mounted, newTree, allocator));
        Assert.Equal("x", ex.Identity);
    }

    [Fact]
    public void Two_siblings_resolving_to_the_same_identity_is_rejected()
    {
        var allocator = NewAllocator();

        // Both rows keyed "A" (e.g. a foreach with a colliding key selector).
        var tree = Root(Row("A", 0, "Alice"), Row("A", -4.3, "Also Alice"));

        Assert.Throws<DuplicateNodeIdentityException>(
            () => Reconciler.Diff(Array.Empty<MountedNode>(), tree, allocator));
    }
}
