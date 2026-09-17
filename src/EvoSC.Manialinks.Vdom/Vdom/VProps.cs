namespace EvoSC.Manialinks.Vdom.Vdom;

/// <summary>
/// Identifies one patchable property, i.e. one that maps to a mutable field on a mounted
/// control and can therefore be changed with a <c>Set</c> op after mount. See the "What is
/// mutable" list in the plan for the game-side field each of these maps to.
///
/// Numeric values are part of the wire contract with the hand-written (not generated, see the
/// plan's L5) <c>Runtime/VdomRuntime.ms</c>, which hardcodes matching <c>#Const</c> values --
/// they are pinned explicitly, and <b>must never be renumbered</b>, only appended to. A build-
/// breaking test (<c>VPropIdTests</c>) exists specifically to catch an accidental renumbering,
/// since nothing else would.
/// </summary>
public enum VPropId
{
    // Common to every control kind (CMlControl).
    Position = 0,
    Size = 1,
    ZIndex = 2,
    Visible = 3,
    RelativeScale = 4,
    RelativeRotation = 5,

    // 6 and 7 were HAlign/VAlign. Moved to VMountProps: there is no verified evidence that
    // CMlControl.HorizontalAlign/VerticalAlign are script-assignable at runtime (only proven as
    // XML attributes), and no real view needs them to vary across renders anyway. Left as a gap
    // rather than reused, per the no-renumbering rule above.
    ToolTip = 8,

    // Quad (CMlQuad).
    BgColor = 9,
    Opacity = 10,
    ImageUrl = 11,

    // Label / Entry (CMlLabel.Value / CMlEntry.Value share the Text slot; the generator picks
    // the right field name from VKind).
    Text = 12,
    TextColor = 13,
    TextSize = 14,
    TextFont = 15
}

/// <summary>One prop that differs between an old and a new <see cref="VNode"/>.</summary>
public readonly record struct VPropChange(VPropId Id, object Value);

/// <summary>
/// The patchable props of a <see cref="VNode"/> -- a sparse bag where <c>null</c> means "not set
/// by this render", not "clear the value". Every render is expected to declare the full set of
/// props relevant to its <see cref="VKind"/> (same discipline as a React/Vue render function),
/// so a field left null across two consecutive renders of the same node is simply never diffed,
/// and a field that is only relevant to a different <see cref="VKind"/> is always null in
/// practice because no builder sets it.
///
/// Deliberately NOT here: <c>class</c>, <c>scriptevents</c>, <c>id</c>, <c>framemodel</c> --
/// those are mount-only and live on <see cref="VMountProps"/> instead, so the type system alone
/// prevents binding them to changing data. See <see cref="VMountProps"/>.
/// </summary>
public sealed record VProps
{
    public static readonly VProps Empty = new();

    // Common (CMlControl).
    public VVec2? Position { get; init; }
    public VVec2? Size { get; init; }
    public double? ZIndex { get; init; }
    public bool? Visible { get; init; }
    public double? RelativeScale { get; init; }
    public double? RelativeRotation { get; init; }

    /// <summary>
    /// NOT YET WIRED UP: neither <c>MountXmlWriter</c> nor <c>Runtime/VdomRuntime.ms</c> handle
    /// this prop -- its XML attribute name is unverified (unlike every other prop here, which
    /// I confirmed against ManiaLink.cs's attribute-stripping regex or direct template usage).
    /// Setting it is currently a silent no-op end to end. No M1 view uses it.
    /// </summary>
    public string? ToolTip { get; init; }

    // Quad.
    public VColor? BgColor { get; init; }
    public double? Opacity { get; init; }
    public string? ImageUrl { get; init; }

    // Label / Entry.
    public string? Text { get; init; }
    public VColor? TextColor { get; init; }
    public double? TextSize { get; init; }
    public string? TextFont { get; init; }

    /// <summary>
    /// Yields one <see cref="VPropChange"/> per field that differs between <paramref name="from"/>
    /// (the previously-mounted props) and this instance (the freshly-rendered props). A field
    /// that is null in both, or unchanged, is not yielded.
    /// </summary>
    public IEnumerable<VPropChange> DiffFrom(VProps from)
    {
        if (Position != from.Position && Position is { } position)
        {
            yield return new VPropChange(VPropId.Position, position);
        }

        if (Size != from.Size && Size is { } size)
        {
            yield return new VPropChange(VPropId.Size, size);
        }

        if (ZIndex != from.ZIndex && ZIndex is { } zIndex)
        {
            yield return new VPropChange(VPropId.ZIndex, zIndex);
        }

        if (Visible != from.Visible && Visible is { } visible)
        {
            yield return new VPropChange(VPropId.Visible, visible);
        }

        if (RelativeScale != from.RelativeScale && RelativeScale is { } relativeScale)
        {
            yield return new VPropChange(VPropId.RelativeScale, relativeScale);
        }

        if (RelativeRotation != from.RelativeRotation && RelativeRotation is { } relativeRotation)
        {
            yield return new VPropChange(VPropId.RelativeRotation, relativeRotation);
        }

        if (ToolTip != from.ToolTip && ToolTip is { } toolTip)
        {
            yield return new VPropChange(VPropId.ToolTip, toolTip);
        }

        if (BgColor != from.BgColor && BgColor is { } bgColor)
        {
            yield return new VPropChange(VPropId.BgColor, bgColor);
        }

        if (Opacity != from.Opacity && Opacity is { } opacity)
        {
            yield return new VPropChange(VPropId.Opacity, opacity);
        }

        if (ImageUrl != from.ImageUrl && ImageUrl is { } imageUrl)
        {
            yield return new VPropChange(VPropId.ImageUrl, imageUrl);
        }

        if (Text != from.Text && Text is { } text)
        {
            yield return new VPropChange(VPropId.Text, text);
        }

        if (TextColor != from.TextColor && TextColor is { } textColor)
        {
            yield return new VPropChange(VPropId.TextColor, textColor);
        }

        if (TextSize != from.TextSize && TextSize is { } textSize)
        {
            yield return new VPropChange(VPropId.TextSize, textSize);
        }

        if (TextFont != from.TextFont && TextFont is { } textFont)
        {
            yield return new VPropChange(VPropId.TextFont, textFont);
        }
    }

    /// <summary>All non-null props of this instance, for the initial <c>Set</c>s after an Alloc.</summary>
    public IEnumerable<VPropChange> AllSet() => DiffFrom(Empty);
}

/// <summary>
/// Props that are fixed for the lifetime of a mounted slot: they are written into the mount-time
/// XML and can never be changed by a patch (<c>class</c> is immutable per-control in ManiaScript;
/// <c>scriptevents</c> looks mount-only since it is absent from the documented CMlControl API
/// surface; <c>HAlign</c>/<c>VAlign</c> are only proven as XML attributes, with no verified
/// runtime-assignable field to patch through -- see the plan's risk table). Kept as a separate
/// type from <see cref="VProps"/> so the compiler, not convention, stops a binding from landing
/// here.
///
/// The <see cref="Reconciler"/> treats a change to these at an otherwise-matched node as an
/// error rather than silently ignoring it -- see <c>Reconciler.MountOnlyPropChangedException</c>.
/// </summary>
public sealed record VMountProps
{
    public static readonly VMountProps Default = new();

    /// <summary>
    /// Whether this control receives MouseOver/MouseOut/MouseClick events. M0's pool spike found
    /// this is a real, measurable cost at volume (not just a documentation gap) -- keep this
    /// false unless a node is genuinely interactive; see the plan's risk table.
    /// </summary>
    public bool ScriptEvents { get; init; }

    public string? Class { get; init; }

    public VAlignHorizontal? HAlign { get; init; }
    public VAlignVertical? VAlign { get; init; }
}
