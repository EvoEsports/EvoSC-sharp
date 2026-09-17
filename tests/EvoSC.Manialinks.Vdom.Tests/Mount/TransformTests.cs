using EvoSC.Manialinks.Vdom.Mount;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Tests.Mount;

public class TransformTests
{
    [Fact]
    public void Identity_composed_with_anything_yields_that_thing_unchanged()
    {
        var child = new Transform(new VVec2(3, 4), 2.5, 30);

        var result = Transform.Identity.Then(child);

        Assert.Equal(child.Translation.X, result.Translation.X, 9);
        Assert.Equal(child.Translation.Y, result.Translation.Y, 9);
        Assert.Equal(child.Scale, result.Scale, 9);
        Assert.Equal(child.RotationDegrees, result.RotationDegrees, 9);
    }

    [Fact]
    public void Pure_translation_composes_by_addition()
    {
        var parent = new Transform(new VVec2(10, 0), 1, 0);
        var child = new Transform(new VVec2(5, 5), 1, 0);

        var result = parent.Then(child);

        Assert.Equal(15, result.Translation.X, 9);
        Assert.Equal(5, result.Translation.Y, 9);
        Assert.Equal(1, result.Scale, 9);
        Assert.Equal(0, result.RotationDegrees, 9);
    }

    [Fact]
    public void Parent_scale_scales_the_childs_local_offset()
    {
        var parent = new Transform(VVec2.Zero, 2, 0);
        var child = new Transform(new VVec2(1, 0), 1, 0);

        var result = parent.Then(child);

        // The child's local (1, 0) offset is scaled by the parent's 2x before being added.
        Assert.Equal(2, result.Translation.X, 9);
        Assert.Equal(0, result.Translation.Y, 9);
        Assert.Equal(2, result.Scale, 9);
    }

    [Fact]
    public void Parent_rotation_rotates_the_childs_local_offset_before_adding()
    {
        var parent = new Transform(new VVec2(10, 10), 1, 90);
        var child = new Transform(new VVec2(1, 0), 1, 0);

        var result = parent.Then(child);

        // (1, 0) rotated 90 degrees CCW -> (0, 1), then added to the parent's translation.
        Assert.Equal(10, result.Translation.X, 6);
        Assert.Equal(11, result.Translation.Y, 6);
        Assert.Equal(90, result.RotationDegrees, 9);
    }

    [Fact]
    public void Scale_and_rotation_both_accumulate_across_a_three_level_chain()
    {
        var grandparent = new Transform(VVec2.Zero, 2, 90);
        var parent = new Transform(new VVec2(1, 0), 3, 0);
        var child = new Transform(new VVec2(0, 1), 1, 45);

        var result = grandparent.Then(parent).Then(child);

        Assert.Equal(6, result.Scale, 9); // 2 * 3 * 1
        Assert.Equal(135, result.RotationDegrees, 9); // 90 + 0 + 45
    }
}
