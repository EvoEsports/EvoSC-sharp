using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Mount;

/// <summary>
/// A position + scale + rotation, composable down a chain of <see cref="VKind.Frame"/> ancestors.
/// Because <c>CMlControl.Parent</c> is immutable (no real reparenting at runtime -- see the
/// plan's "Hard walls"), a pooled control can never inherit a parent frame's transform through
/// the game's own control hierarchy. Instead, every virtual <see cref="VKind.Frame"/> in the
/// authored tree is purely a transform contributor: <see cref="LayoutFlattener"/> composes one
/// of these down each ancestor chain and bakes the result into each leaf's absolute
/// <see cref="VPropId.Position"/> / <see cref="VPropId.RelativeScale"/> /
/// <see cref="VPropId.RelativeRotation"/> before the <see cref="Reconciler"/> ever sees it.
/// </summary>
public readonly record struct Transform(VVec2 Translation, double Scale, double RotationDegrees)
{
    public static readonly Transform Identity = new(VVec2.Zero, 1.0, 0.0);

    /// <summary>
    /// Composes a child transform (declared in the child's own local space) onto this one,
    /// producing the child's transform in the same space as this transform. Order matters:
    /// <c>parent.Then(child)</c>, not the reverse.
    /// </summary>
    public Transform Then(Transform child)
    {
        var rotatedAndScaled = child.Translation.Rotate(RotationDegrees) * Scale;

        return new Transform(
            Translation + rotatedAndScaled,
            Scale * child.Scale,
            RotationDegrees + child.RotationDegrees);
    }
}
