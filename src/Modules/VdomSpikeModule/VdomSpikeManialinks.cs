using System.Text;

namespace EvoSC.Modules.Official.VdomSpikeModule;

/// <summary>
/// Hand-written manialink XML for the M0 VDOM transport spike. Deliberately bypasses
/// ManiaTemplates / IManialinkManager so the spike tests only the game's own APIs.
///
/// Three pages, each with a fixed id so they can be mounted/re-sent independently:
///   - <see cref="ViewPageName"/>: mounted once. Shows a tick counter (proves the script loop
///     survives patches), a hover counter + color swap (proves interaction state survives), and
///     a rolling log of every patch it has observed via a "for LocalUser" extended variable.
///   - <see cref="PatchPageName"/>: re-sent on every "//vdomspikepatch" / "//vdomspikeburst"
///     call. Writes VdomSpike_Value / VdomSpike_Seq "for LocalUser" and exits immediately.
///   - <see cref="PoolPageName"/>: mounts N generic pooled quads to measure mount payload size
///     and event volume at realistic pool sizes (200 / 1,000 / 2,000).
/// </summary>
public static class VdomSpikeManialinks
{
    public const string ViewPageName = "EvoSC_VdomSpike_View";
    public const string PatchPageName = "EvoSC_VdomSpike_Patch";
    public const string PoolPageName = "EvoSC_VdomSpike_Pool";

    public static string BuildViewPage()
    {
        return $$"""
            <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
            <manialink version="3" id="{{ViewPageName}}" name="EvoSC#-VdomSpike-View">
                <quad id="hoverbox" class="hoverbox" pos="-70 70" size="24 10" bgcolor="333" scriptevents="1" data-id="hoverbox"/>
                <label id="hoverlabel" pos="-70 84" size="40 4" text="Hover: 0" textsize="1" textcolor="FFF" halign="left"/>
                <label id="ticklabel" pos="-70 58" size="60 4" text="Ticks: 0" textsize="1" textcolor="FFF" halign="left"/>
                <label id="patchlabel" pos="-70 52" size="80 4" text="Patch: (none yet)" textsize="1" textcolor="FFF" halign="left"/>
                <label id="loglabel" pos="-70 44" size="90 40" text="" textsize="0.8" textcolor="AAA" valign="top" halign="left"/>
                <script><!--
            main() {
                declare Integer Ticks = 0;
                declare Integer LastSeenSeq = -1;
                declare Integer HoverCount = 0;
                declare Text LogText = "";

                declare CMlLabel HoverLabel <=> (Page.GetFirstChild("hoverlabel") as CMlLabel);
                declare CMlLabel TickLabel <=> (Page.GetFirstChild("ticklabel") as CMlLabel);
                declare CMlLabel PatchLabel <=> (Page.GetFirstChild("patchlabel") as CMlLabel);
                declare CMlLabel LogLabel <=> (Page.GetFirstChild("loglabel") as CMlLabel);
                declare CMlQuad HoverBox <=> (Page.GetFirstChild("hoverbox") as CMlQuad);

                // "for LocalUser" is shared across manialink pages on the same client. This is
                // the exact mechanism M0 is verifying: the patch page (a separate manialink,
                // re-sent repeatedly) writes these two, and this page -- mounted once, never
                // re-sent -- reads them every tick without its own script ever restarting.
                declare Text VdomSpike_Value for LocalUser;
                declare Integer VdomSpike_Seq for LocalUser = -1;

                while (True) {
                    yield;
                    Ticks += 1;
                    TickLabel.Value = "Ticks: " ^ Ticks ^ "  (should climb continuously, never reset by a patch)";

                    if (VdomSpike_Seq != LastSeenSeq) {
                        LastSeenSeq = VdomSpike_Seq;
                        PatchLabel.Value = "Patch: " ^ VdomSpike_Value ^ "  (seq " ^ VdomSpike_Seq ^ ")";
                        LogText = "seq " ^ VdomSpike_Seq ^ " -> \"" ^ VdomSpike_Value ^ "\" @tick " ^ Ticks ^ "\n" ^ LogText;
                        LogLabel.Value = LogText;
                    }

                    foreach (Event in PendingEvents) {
                        switch (Event.Type) {
                            case CMlScriptEvent::Type::MouseOver: {
                                if (Event.Control.HasClass("hoverbox")) {
                                    HoverCount += 1;
                                    HoverLabel.Value = "Hover: " ^ HoverCount ^ "  (should keep counting up across patches)";
                                    HoverBox.BgColor = <0., 1., 0.>;
                                }
                            }
                            case CMlScriptEvent::Type::MouseOut: {
                                if (Event.Control.HasClass("hoverbox")) {
                                    HoverBox.BgColor = <0.2, 0.2, 0.2>;
                                }
                            }
                        }
                    }
                }
            }
            --></script>
            </manialink>
            """;
    }

    public static string BuildPatchPage(string value, int seq)
    {
        var escapedValue = EscapeManiaScriptString(value);

        return $$"""
            <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
            <manialink version="3" id="{{PatchPageName}}" name="EvoSC#-VdomSpike-Patch">
                <script><!--
            main() {
                declare Text VdomSpike_Value for LocalUser;
                declare Integer VdomSpike_Seq for LocalUser;

                VdomSpike_Value = "{{escapedValue}}";
                VdomSpike_Seq = {{seq}};
            }
            --></script>
            </manialink>
            """;
    }

    /// <summary>
    /// Mounts a pool of <paramref name="count"/> generic scriptevents-enabled quads in a grid,
    /// to measure mount payload size / visible hitch at realistic pool sizes.
    /// </summary>
    public static string BuildPoolPage(int count)
    {
        var cols = Math.Max(1, (int)Math.Ceiling(Math.Sqrt(count)));
        var rows = (int)Math.Ceiling(count / (double)cols);
        var cellSize = Math.Clamp(300.0 / cols, 0.5, 4.0);
        var gap = cellSize * 0.1;
        var startX = -((cols - 1) * cellSize) / 2.0;
        var startY = ((rows - 1) * cellSize) / 2.0;

        var quads = new StringBuilder(count * 96);
        for (var i = 0; i < count; i++)
        {
            var col = i % cols;
            var row = i / cols;
            var x = startX + col * cellSize;
            var y = startY - row * cellSize;
            var color = (col + row) % 2 == 0 ? "0AF" : "F80";

            quads.Append("<quad pos=\"")
                .Append(x.ToString("0.###"))
                .Append(' ')
                .Append(y.ToString("0.###"))
                .Append("\" size=\"")
                .Append((cellSize - gap).ToString("0.###"))
                .Append(' ')
                .Append((cellSize - gap).ToString("0.###"))
                .Append("\" bgcolor=\"")
                .Append(color)
                .Append("\" scriptevents=\"1\"/>\n");
        }

        return $$"""
            <?xml version="1.0" encoding="utf-8" standalone="yes" ?>
            <manialink version="3" id="{{PoolPageName}}" name="EvoSC#-VdomSpike-Pool">
                <label id="poolstats" pos="-159 88" size="120 5" text="mounting..." textsize="1" textcolor="FFF" halign="left"/>
                {{quads}}
                <script><!--
            main() {
                declare Integer Ticks = 0;
                declare Integer EventCount = 0;
                declare CMlLabel StatsLabel <=> (Page.GetFirstChild("poolstats") as CMlLabel);

                StatsLabel.Value = "Pool mounted: {{count}} controls. Move the mouse across the grid and watch for hitching.";

                while (True) {
                    yield;
                    Ticks += 1;

                    foreach (Event in PendingEvents) {
                        EventCount += 1;
                    }

                    if (Ticks % 10 == 0) {
                        StatsLabel.Value = "{{count}} controls | ticks: " ^ Ticks ^ " | events seen: " ^ EventCount;
                    }
                }
            }
            --></script>
            </manialink>
            """;
    }

    private static string EscapeManiaScriptString(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");
}
