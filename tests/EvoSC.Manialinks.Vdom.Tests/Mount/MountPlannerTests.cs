using EvoSC.Manialinks.Vdom.Mount;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Mount;

public class MountPlannerTests
{
    private static readonly Dictionary<(VKind, bool), int> Budget = new()
    {
        [(VKind.Quad, false)] = 3,
        [(VKind.Quad, true)] = 2,
        [(VKind.Label, false)] = 3
    };

    [Fact]
    public void Used_slots_are_visible_with_their_rendered_props_and_unused_slots_are_hidden_placeholders()
    {
        var tree = VNode.Group(null, new[]
        {
            VNode.Leaf(VKind.Quad, "bg", new VProps
            {
                Position = new VVec2(1, 2), Size = new VVec2(10, 4), BgColor = VColor.FromHex("0AF")
            }),
            VNode.Leaf(VKind.Label, "name", new VProps { Text = "Alice" })
        });

        var result = MountPlanner.Mount(Budget, tree);

        // Total controls = sum of every bucket's capacity: 3+2 quads, 3 labels.
        Assert.Equal(5, result.BodyXml.Split("<quad").Length - 1);
        Assert.Equal(3, result.BodyXml.Split("<label").Length - 1);

        // The used quad (slot 0, first non-interactive quad) is visible with its real props.
        // hidden/scriptevents are omitted entirely when they'd be the default (visible,
        // non-interactive) -- see MountXmlWriter's note on why.
        Assert.Contains("id=\"q0\" pos=\"1 2\" size=\"10 4\"", result.BodyXml);
        Assert.DoesNotContain("id=\"q0\" pos=\"1 2\" size=\"10 4\" hidden", result.BodyXml);
        Assert.Contains("bgcolor=\"00AAFF\"", result.BodyXml);

        // The remaining quad slots are hidden placeholders.
        Assert.Contains("id=\"q1\" pos=\"0 0\" size=\"1 1\" hidden=\"1\"", result.BodyXml);
        Assert.Contains("id=\"q2\" pos=\"0 0\" size=\"1 1\" hidden=\"1\"", result.BodyXml);

        // Interactive quads (slots 3-4, per PoolLayout's non-interactive-then-interactive order)
        // are unused here, so hidden, but still carry scriptevents="1".
        Assert.Contains("id=\"q3\" pos=\"0 0\" size=\"1 1\" hidden=\"1\" scriptevents=\"1\"", result.BodyXml);
        Assert.Contains("id=\"q4\" pos=\"0 0\" size=\"1 1\" hidden=\"1\" scriptevents=\"1\"", result.BodyXml);

        Assert.Contains("id=\"l0\"", result.BodyXml);
        Assert.Contains("text=\"Alice\"", result.BodyXml);
    }

    [Fact]
    public void The_returned_mount_state_feeds_directly_into_the_next_diff()
    {
        var tree = VNode.Group(null, new[] { VNode.Leaf(VKind.Quad, "bg", new VProps()) });
        var result = MountPlanner.Mount(Budget, tree);

        var updatedTree = VNode.Group(null, new[]
        {
            VNode.Leaf(VKind.Quad, "bg", new VProps { BgColor = VColor.FromHex("F00") })
        });

        var (ops, _) = Reconciler.Diff(result.Mounted, updatedTree, result.Allocator);

        Assert.Contains(ops, op => op is VPatchOp.Set { Prop: VPropId.BgColor });
        Assert.DoesNotContain(ops, op => op is VPatchOp.Alloc or VPatchOp.Free);
    }
}
