using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Mount;

/// <summary>
/// Turns an author-facing, nested <see cref="VNode"/> tree (positions authored relative to their
/// immediate parent, exactly as you'd write nested &lt;frame&gt; XML) into a flat list of
/// <see cref="FlatNode"/>s with fully composed absolute positions and stable cross-render
/// identities -- the shape the <see cref="Reconciler"/> actually diffs.
///
/// This exists because <c>CMlControl.Parent</c> is immutable (see the plan's "Hard walls"): a
/// pooled control can never really inherit a parent frame's transform at runtime, so the
/// composition this class does at diff time is doing the job the game's own control hierarchy
/// would otherwise do for free.
/// </summary>
public static class LayoutFlattener
{
    public static IReadOnlyList<FlatNode> Flatten(VNode root)
    {
        var result = new List<FlatNode>();
        var path = new List<object>();

        Walk(root, Transform.Identity, path, result);

        return result;
    }

    private static void Walk(VNode node, Transform parentTransform, List<object> path, List<FlatNode> result)
    {
        var localTransform = new Transform(
            node.Props.Position ?? VVec2.Zero,
            node.Props.RelativeScale ?? 1.0,
            node.Props.RelativeRotation ?? 0.0);
        var combined = parentTransform.Then(localTransform);

        // A Frame with no container need is a pure grouping/transform node: it contributes its
        // transform to its children but is itself erased -- it never consumes a pool slot.
        var isVirtualGroup = node.Kind == VKind.Frame && node.Container == VContainerNeed.None;

        if (!isVirtualGroup)
        {
            // Position always needs an explicit value -- a reused slot may carry a stale
            // position from whatever it last displayed. Scale/rotation are left unset when they
            // are the identity value (1.0 / 0.0) rather than baked into every single leaf, since
            // that is already a freshly-shown control's natural state and would otherwise put a
            // needless Set on every mount.
            var bakedProps = node.Props with
            {
                Position = combined.Translation,
                RelativeScale = combined.Scale == 1.0 ? null : combined.Scale,
                RelativeRotation = combined.RotationDegrees == 0.0 ? null : combined.RotationDegrees
            };

            result.Add(new FlatNode(path.ToArray(), node.Kind, node.MountProps, bakedProps));
        }

        if (node.Kind != VKind.Frame)
        {
            if (node.Children.Count > 0)
            {
                throw new InvalidOperationException(
                    $"{node.Kind} nodes cannot have children (node key: {node.Key}).");
            }

            return;
        }

        // A real container frame (M2 -- clip/scroll) is a genuine reparenting point once it
        // exists, so its children's transform starts fresh relative to it. A virtual group
        // (M1's only case) passes the composed transform straight through.
        var childBaseTransform = isVirtualGroup ? combined : Transform.Identity;

        for (var i = 0; i < node.Children.Count; i++)
        {
            var child = node.Children[i];
            path.Add(child.Key ?? i);
            Walk(child, childBaseTransform, path, result);
            path.RemoveAt(path.Count - 1);
        }
    }
}
