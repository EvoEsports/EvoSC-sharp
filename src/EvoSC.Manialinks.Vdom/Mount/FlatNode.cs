using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Mount;

/// <summary>
/// One leaf produced by <see cref="LayoutFlattener"/>: a control that will actually occupy a
/// pool slot, with its <see cref="Props"/> already carrying the fully composed absolute
/// Position / RelativeScale / RelativeRotation (see <see cref="Transform"/>) -- the
/// <see cref="Reconciler"/> never has to know about ancestor frames at all.
/// </summary>
/// <param name="IdentityPath">
/// This leaf's identity across renders: the chain of ancestor keys (falling back to positional
/// index at levels with no explicit key) down to and including this leaf's own segment. Two
/// flattened trees' leaves are matched by comparing this, via <see cref="Identity"/>.
/// </param>
public readonly record struct FlatNode(
    IReadOnlyList<object> IdentityPath,
    VKind Kind,
    VMountProps MountProps,
    VProps Props)
{
    /// <summary>The string form of <see cref="IdentityPath"/> used for matching and as a map key.</summary>
    public string Identity => string.Join('/', IdentityPath.Select(FormatSegment));

    private static string FormatSegment(object segment) => segment switch
    {
        string s => s,
        IFormattable f => f.ToString(null, System.Globalization.CultureInfo.InvariantCulture),
        _ => segment.ToString() ?? ""
    };
}
