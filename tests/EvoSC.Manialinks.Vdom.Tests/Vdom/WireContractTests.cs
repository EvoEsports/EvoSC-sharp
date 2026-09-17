using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Vdom;

/// <summary>
/// <see cref="VKind"/> and <see cref="VPropId"/>'s numeric values are part of the wire contract
/// with the hand-written, non-generated <c>Runtime/VdomRuntime.ms</c> (see the plan's L5) --
/// nothing in the type system stops someone from reordering or inserting an enum member and
/// silently shifting every value after it. This test is the only thing that would catch that, so
/// treat any failure here as "go update VdomRuntime.ms's matching #Const block", never as
/// "update this test to match".
/// </summary>
public class WireContractTests
{
    [Fact]
    public void VKind_values_are_pinned()
    {
        Assert.Equal(0, (int)VKind.Quad);
        Assert.Equal(1, (int)VKind.Label);
        Assert.Equal(2, (int)VKind.Entry);
        Assert.Equal(3, (int)VKind.Frame);
    }

    [Fact]
    public void VPropId_values_are_pinned()
    {
        Assert.Equal(0, (int)VPropId.Position);
        Assert.Equal(1, (int)VPropId.Size);
        Assert.Equal(2, (int)VPropId.ZIndex);
        Assert.Equal(3, (int)VPropId.Visible);
        Assert.Equal(4, (int)VPropId.RelativeScale);
        Assert.Equal(5, (int)VPropId.RelativeRotation);
        // 6 (HAlign) and 7 (VAlign) are a deliberate gap -- moved to VMountProps, see VProps.cs.
        Assert.Equal(8, (int)VPropId.ToolTip);
        Assert.Equal(9, (int)VPropId.BgColor);
        Assert.Equal(10, (int)VPropId.Opacity);
        Assert.Equal(11, (int)VPropId.ImageUrl);
        Assert.Equal(12, (int)VPropId.Text);
        Assert.Equal(13, (int)VPropId.TextColor);
        Assert.Equal(14, (int)VPropId.TextSize);
        Assert.Equal(15, (int)VPropId.TextFont);
    }
}
