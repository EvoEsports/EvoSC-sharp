using EvoSC.Manialinks.Vdom.Builder;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Builder;

public class VTests
{
    [Fact]
    public void Quad_maps_named_args_onto_the_right_VProps_fields()
    {
        var node = V.Quad(
            key: "bg",
            position: new VVec2(1, 2),
            size: new VVec2(10, 5),
            bgColor: VColor.FromHex("0AF"),
            opacity: 0.5,
            scriptEvents: true,
            cssClass: "hoverbox");

        Assert.Equal(VKind.Quad, node.Kind);
        Assert.Equal("bg", node.Key);
        Assert.Equal(new VVec2(1, 2), node.Props.Position);
        Assert.Equal(new VVec2(10, 5), node.Props.Size);
        Assert.Equal(VColor.FromHex("0AF"), node.Props.BgColor);
        Assert.Equal(0.5, node.Props.Opacity);
        Assert.True(node.MountProps.ScriptEvents);
        Assert.Equal("hoverbox", node.MountProps.Class);
        Assert.Empty(node.Children);
    }

    [Fact]
    public void Quad_leaves_unset_props_null_not_defaulted()
    {
        var node = V.Quad();

        Assert.Null(node.Props.Position);
        Assert.Null(node.Props.BgColor);
        Assert.Null(node.Props.ImageUrl);
        Assert.False(node.MountProps.ScriptEvents);
    }

    [Fact]
    public void Label_maps_text_props_and_alignment_onto_mount_props()
    {
        var node = V.Label(
            text: "Alice",
            textColor: VColor.FromHex("FFF"),
            textSize: 2,
            hAlign: VAlignHorizontal.Center,
            vAlign: VAlignVertical.Top);

        Assert.Equal(VKind.Label, node.Kind);
        Assert.Equal("Alice", node.Props.Text);
        Assert.Equal(VColor.FromHex("FFF"), node.Props.TextColor);
        Assert.Equal(2.0, node.Props.TextSize);
        Assert.Equal(VAlignHorizontal.Center, node.MountProps.HAlign);
        Assert.Equal(VAlignVertical.Top, node.MountProps.VAlign);
    }

    [Fact]
    public void Entry_maps_the_same_text_props_as_Label()
    {
        var node = V.Entry(text: "type here", textSize: 1.5);

        Assert.Equal(VKind.Entry, node.Kind);
        Assert.Equal("type here", node.Props.Text);
        Assert.Equal(1.5, node.Props.TextSize);
    }

    [Fact]
    public void Frame_with_explicit_children_list_carries_position_and_container_need()
    {
        var child = V.Quad();
        var frame = V.Frame("row", new[] { child }, position: new VVec2(0, -4.3), container: VContainerNeed.Clip);

        Assert.Equal(VKind.Frame, frame.Kind);
        Assert.Equal("row", frame.Key);
        Assert.Equal(new VVec2(0, -4.3), frame.Props.Position);
        Assert.Equal(VContainerNeed.Clip, frame.Container);
        Assert.Same(child, Assert.Single(frame.Children));
    }

    [Fact]
    public void Frame_params_overload_groups_inline_children_with_no_props()
    {
        var quad = V.Quad();
        var label = V.Label();
        var frame = V.Frame("row", quad, label);

        Assert.Equal(VKind.Frame, frame.Kind);
        Assert.Equal(VProps.Empty, frame.Props);
        Assert.Equal(new[] { quad, label }, frame.Children);
    }
}
