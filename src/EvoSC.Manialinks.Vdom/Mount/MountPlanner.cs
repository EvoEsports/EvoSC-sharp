using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Mount;

/// <summary>The result of planning and performing an initial mount.</summary>
/// <param name="BodyXml">
/// The pool's control tags (see <see cref="MountXmlWriter"/>) -- everything that goes inside the
/// outer <c>&lt;manialink&gt;</c> tag; the caller wraps this with the page envelope and the
/// embedded <c>VdomRuntime.ms</c>.
/// </param>
/// <param name="Allocator">
/// The allocator to keep alongside this view's mounted state and reuse for every subsequent
/// <see cref="Reconciler.Diff"/> call.
/// </param>
/// <param name="Mounted">The initial mount's state -- pass this into the next diff as <c>previouslyMounted</c>.</param>
public readonly record struct MountResult(string BodyXml, SlotAllocator Allocator, IReadOnlyList<MountedNode> Mounted);

/// <summary>
/// Ties <see cref="PoolLayout"/>, <see cref="SlotAllocator"/>, <see cref="Reconciler"/> and
/// <see cref="MountXmlWriter"/> together for the one-time initial mount of a view: the first
/// render's diff (against an empty previous state) doubles as the source of truth for which pool
/// slots the mount XML should show as already in use.
/// </summary>
public static class MountPlanner
{
    public static MountResult Mount(
        IReadOnlyDictionary<(VKind Kind, bool Interactive), int> poolBudget,
        VNode initialTree)
    {
        var layout = PoolLayout.Compute(poolBudget);
        var allocator = new SlotAllocator(layout);
        var (_, mounted) = Reconciler.Diff(Array.Empty<MountedNode>(), initialTree, allocator);
        var body = MountXmlWriter.WriteBody(layout, mounted);

        return new MountResult(body, allocator, mounted);
    }
}
