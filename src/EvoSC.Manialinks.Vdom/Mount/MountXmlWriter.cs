using System.Text;
using EvoSC.Manialinks.Vdom.Vdom;

namespace EvoSC.Manialinks.Vdom.Mount;

/// <summary>
/// Renders the mount-time XML for a view's control pool: one generic control per slot in
/// <see cref="PoolLayout"/>'s order, hidden by default, with whichever slots the initial render
/// actually used carrying that render's real values instead of placeholders -- so mounting and
/// the first render are one XML send, not a mount followed immediately by a patch.
///
/// Attribute names are taken from <c>ManiaLink.cs</c>'s default-attribute-stripping regex in the
/// ManiaTemplates engine (it strips e.g. <c>z-index="0"</c>, <c>scale="1"</c>, <c>rot="0"</c> --
/// which only makes sense if those are real attribute names), not guessed.
/// </summary>
public static class MountXmlWriter
{
    /// <summary>
    /// Writes every pooled control's tag, in <paramref name="layout"/> order, from
    /// <paramref name="mounted"/> (the result of the initial <see cref="Reconciler.Diff"/>
    /// against an empty previous state -- see <see cref="MountPlanner"/>). Each slot gets a
    /// short deterministic id (e.g. <c>"q5"</c>) that <c>VdomRuntime.ms</c> resolves once at
    /// mount via <c>Page.GetFirstChild</c> -- not a bulk <c>GetClassChildren</c> scan, since that
    /// would depend on an unverified ordering guarantee for something this central.
    /// </summary>
    public static string WriteBody(IReadOnlyList<PoolBucketRange> layout, IReadOnlyList<MountedNode> mounted)
    {
        var bySlot = new Dictionary<(VKind Kind, int Slot), MountedNode>(mounted.Count);

        foreach (var node in mounted)
        {
            bySlot[(node.Flat.Kind, node.Slot)] = node;
        }

        var xml = new StringBuilder();

        foreach (var range in layout)
        {
            for (var i = 0; i < range.Count; i++)
            {
                var slot = range.Start + i;
                var isUsed = bySlot.TryGetValue((range.Kind, slot), out var node);

                AppendControl(xml, range.Kind, slot, range.Interactive, isUsed ? node : null);
            }
        }

        return xml.ToString();
    }

    /// <summary>The deterministic per-slot id <c>VdomRuntime.ms</c> resolves at mount.</summary>
    public static string ControlId(VKind kind, int slot) => $"{KindPrefix(kind)}{slot}";

    private static string KindPrefix(VKind kind) => kind switch
    {
        VKind.Quad => "q",
        VKind.Label => "l",
        VKind.Entry => "e",
        VKind.Frame => "f",
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };

    private static void AppendControl(StringBuilder xml, VKind kind, int slot, bool interactive, MountedNode? used)
    {
        if (kind == VKind.Frame)
        {
            // A pooled Frame only exists for M2's clip/scroll containers, which no M1 view uses
            // -- see LayoutFlattener, which never emits a non-virtual Frame FlatNode today.
            throw new NotSupportedException(
                "Pooled Frame controls are M2 (clip/scroll containers); no M1 view should reach this.");
        }

        var props = used?.Flat.Props;
        var mountProps = used?.Flat.MountProps;
        var visible = used.HasValue && (props!.Visible ?? true);
        var pos = props?.Position ?? VVec2.Zero;
        var size = props?.Size ?? new VVec2(1, 1);

        xml.Append('<').Append(TagName(kind))
            .Append(" id=\"").Append(ControlId(kind, slot)).Append('"')
            .Append(" pos=\"").Append(pos).Append('"')
            .Append(" size=\"").Append(size).Append('"');

        // Only emitted when non-default -- matching ManiaLink.cs's own default-attribute
        // stripping in the old engine (it strips scriptevents="0" unconditionally, and never
        // emits redundant defaults generally). A visible, non-interactive control -- the common
        // case -- gets neither attribute at all, exactly matching the one label recipe confirmed
        // to render visible text in-game (M0's spike, src/Modules/VdomSpikeModule): no `hidden`,
        // no `scriptevents` on its always-shown label.
        if (!visible)
        {
            xml.Append(" hidden=\"1\"");
        }

        if (interactive)
        {
            xml.Append(" scriptevents=\"1\"");
        }

        if (mountProps?.Class is { } cls)
        {
            xml.Append(" class=\"").Append(XmlEscape(cls)).Append('"');
        }

        if (mountProps?.HAlign is { } hAlign)
        {
            xml.Append(" halign=\"").Append(FormatHAlign(hAlign)).Append('"');
        }

        if (mountProps?.VAlign is { } vAlign)
        {
            xml.Append(" valign=\"").Append(FormatVAlign(vAlign)).Append('"');
        }

        if (props?.ZIndex is { } zIndex)
        {
            xml.Append(" z-index=\"").Append(zIndex.ToString("0.###")).Append('"');
        }

        if (props?.RelativeScale is { } scale)
        {
            xml.Append(" scale=\"").Append(scale.ToString("0.###")).Append('"');
        }

        if (props?.RelativeRotation is { } rotation)
        {
            xml.Append(" rot=\"").Append(rotation.ToString("0.###")).Append('"');
        }

        switch (kind)
        {
            case VKind.Quad:
                if (props?.BgColor is { } bgColor)
                {
                    xml.Append(" bgcolor=\"").Append(bgColor.ToHex()).Append('"');
                }

                if (props?.Opacity is { } quadOpacity)
                {
                    xml.Append(" opacity=\"").Append(quadOpacity.ToString("0.###")).Append('"');
                }

                if (props?.ImageUrl is { } imageUrl)
                {
                    xml.Append(" image=\"").Append(XmlEscape(imageUrl)).Append('"');
                }

                break;

            case VKind.Label:
            case VKind.Entry:
                if (props?.Text is { } text)
                {
                    xml.Append(" text=\"").Append(XmlEscape(text)).Append('"');
                }

                if (props?.TextColor is { } textColor)
                {
                    xml.Append(" textcolor=\"").Append(textColor.ToHex()).Append('"');
                }

                if (props?.TextSize is { } textSize)
                {
                    xml.Append(" textsize=\"").Append(textSize.ToString("0.###")).Append('"');
                }

                if (props?.TextFont is { } textFont)
                {
                    xml.Append(" textfont=\"").Append(XmlEscape(textFont)).Append('"');
                }

                if (props?.Opacity is { } labelOpacity)
                {
                    xml.Append(" opacity=\"").Append(labelOpacity.ToString("0.###")).Append('"');
                }

                break;
        }

        xml.Append("/>\n");
    }

    private static string TagName(VKind kind) => kind switch
    {
        VKind.Quad => "quad",
        VKind.Label => "label",
        VKind.Entry => "entry",
        VKind.Frame => "frame",
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };

    private static string FormatHAlign(VAlignHorizontal align) => align switch
    {
        VAlignHorizontal.Left => "left",
        VAlignHorizontal.Center => "center",
        VAlignHorizontal.Right => "right",
        _ => throw new ArgumentOutOfRangeException(nameof(align), align, null)
    };

    private static string FormatVAlign(VAlignVertical align) => align switch
    {
        VAlignVertical.Top => "top",
        VAlignVertical.Center => "center",
        VAlignVertical.Bottom => "bottom",
        _ => throw new ArgumentOutOfRangeException(nameof(align), align, null)
    };

    private static string XmlEscape(string text) => text
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;")
        .Replace("\"", "&quot;");
}
