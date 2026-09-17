using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Builder;

/// <summary>
/// Typed, ergonomic <see cref="VNode"/> construction helpers -- what a view's <c>Render</c>
/// implementation actually calls, instead of the raw <see cref="VNode.Leaf"/>/<see cref="VNode.Group"/>
/// factories. Every parameter is nullable and defaults to <c>null</c>, never a concrete zero
/// value, so an unset prop stays genuinely unset (see <see cref="VProps"/>'s "sparse bag"
/// semantics) rather than looking identical to an explicit zero.
///
/// Found in-game: XML document order is <b>not</b> a reliable paint-order signal for this
/// engine's flat, pooled control structure (every control is a direct sibling of
/// <c>&lt;manialink&gt;</c>, unlike the old engine's more deeply-nested-per-frame templates).
/// With every control at the default ZIndex, quads painted over labels regardless of emission
/// order. Stacking must be controlled explicitly via <see cref="Quad"/>/<see cref="Label"/>'s
/// <c>zIndex</c> parameter -- there is no implicit "later = on top" to lean on.
/// </summary>
public static class V
{
    /// <summary>
    /// Every label/entry in the existing (old-engine) templates always has an explicit font,
    /// either directly or via a stylesheet <c>class</c> -- and this engine has no stylesheet
    /// system at all (<c>class</c> is an inert attribute here, see <c>MountXmlWriter</c>). Found
    /// the hard way: an unset <c>textfont</c> renders completely invisible text, not a fallback
    /// font -- so <see cref="Label"/>/<see cref="Entry"/> default to this rather than to
    /// <c>null</c>. "GameFont*" is Trackmania's own built-in family, available regardless of
    /// theme (unlike Rajdhani/Oswald/Roboto, which the theme must supply) -- a deliberate,
    /// no-theme-integration-yet placeholder; see <c>EvoSC.Manialinks.Util.FontManialinkHelper</c>
    /// for the real per-theme mapping this bypasses.
    /// </summary>
    public const string DefaultFont = "GameFontSemiBold";

    public static VNode Quad(
        object? key = null,
        VVec2? position = null,
        VVec2? size = null,
        VColor? bgColor = null,
        double? opacity = null,
        string? imageUrl = null,
        double? zIndex = null,
        bool scriptEvents = false,
        string? cssClass = null) =>
        VNode.Leaf(
            VKind.Quad,
            key,
            new VProps
            {
                Position = position, Size = size, BgColor = bgColor, Opacity = opacity, ImageUrl = imageUrl,
                ZIndex = zIndex
            },
            new VMountProps { ScriptEvents = scriptEvents, Class = cssClass });

    public static VNode Label(
        object? key = null,
        VVec2? position = null,
        VVec2? size = null,
        string? text = null,
        VColor? textColor = null,
        double? textSize = null,
        string? textFont = DefaultFont,
        double? opacity = null,
        double? zIndex = null,
        VAlignHorizontal? hAlign = null,
        VAlignVertical? vAlign = null,
        bool scriptEvents = false,
        string? cssClass = null) =>
        VNode.Leaf(
            VKind.Label,
            key,
            new VProps
            {
                Position = position, Size = size, Text = text, TextColor = textColor, TextSize = textSize,
                TextFont = textFont, Opacity = opacity, ZIndex = zIndex
            },
            new VMountProps { ScriptEvents = scriptEvents, Class = cssClass, HAlign = hAlign, VAlign = vAlign });

    /// <summary>
    /// An editable text entry. Deliberately narrower than <see cref="Label"/> -- only Text and
    /// TextSize are wired up end to end (see <c>Runtime/VdomRuntime.ms</c>'s
    /// <c>__VdomApplyEntry</c>); CMlEntry's exact field set beyond Value was not independently
    /// verified, and no M1 view uses one.
    /// </summary>
    public static VNode Entry(
        object? key = null,
        VVec2? position = null,
        VVec2? size = null,
        string? text = null,
        double? textSize = null,
        string? textFont = DefaultFont,
        double? zIndex = null,
        bool scriptEvents = false,
        string? cssClass = null) =>
        VNode.Leaf(
            VKind.Entry,
            key,
            new VProps
            {
                Position = position, Size = size, Text = text, TextSize = textSize, TextFont = textFont,
                ZIndex = zIndex
            },
            new VMountProps { ScriptEvents = scriptEvents, Class = cssClass });

    /// <summary>
    /// A pure grouping/transform node -- see <c>Mount/LayoutFlattener.cs</c>. Consumes no pool
    /// slot unless <paramref name="container"/> is set (M2; no M1 view uses this).
    /// </summary>
    public static VNode Frame(
        object? key,
        IReadOnlyList<VNode> children,
        VVec2? position = null,
        VContainerNeed container = VContainerNeed.None) =>
        VNode.Group(key, children, position is null ? null : new VProps { Position = position }, container);

    /// <summary>Convenience overload for inline children with no props of their own.</summary>
    public static VNode Frame(object? key, params VNode[] children) => VNode.Group(key, children);
}
