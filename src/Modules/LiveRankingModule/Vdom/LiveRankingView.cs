using EvoSC.Common.Util;
using EvoSC.Common.Util.Manialinks;
using EvoSC.Manialinks.Vdom.Builder;
using EvoSC.Manialinks.Vdom.Vdom;
using EvoSC.Manialinks.Vdom.Views;
using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Vdom;

/// <summary>
/// M1's vertical-slice port of <c>LiveRankingModule/Templates/LiveRanking.mt</c> +
/// <c>Components/LiveRankingRecordRow.mt</c> to the reactive engine. Deliberately simplified,
/// not a pixel-perfect port: no theme integration (M4's job -- colors here are hardcoded
/// placeholders), no rounded-corner <c>Panel</c>/<c>QuarterCircle</c> chrome, no per-row
/// "self" highlight (the one dynamic-<c>class</c> pattern the plan's M1 section calls out as not
/// surviving the port as-is). What this exists to prove is the pipeline: a keyed list of rows
/// reacting to real score updates through mount -> diff -> patch -> apply -> ack, on the actual
/// data LiveRankingService produces every scores event.
/// </summary>
public sealed class LiveRankingView : IVdomView
{
    private const double RowHeight = 4.0;
    private const double RowSpacing = 0.3;
    private const double HeaderHeight = 4.5;
    private const double BodyOffset = -(HeaderHeight + 0.3);
    private const double WidgetPadding = 1.5;

    private static readonly VColor HeaderBg = VColor.FromHex("1A1A1A");
    private static readonly VColor BodyBg = VColor.FromHex("101010");
    private static readonly VColor RowBg = VColor.FromHex("2A2A2A");
    private static readonly VColor TextColor = VColor.FromHex("FFFFFF");

    // Explicit paint-order layers -- found in-game that XML document order is not a reliable
    // paint-order signal for this engine's flat pooled controls (unlike the old engine's more
    // deeply-nested-per-frame templates); see V.cs's class doc. Chrome sits below rows, rows sit
    // below all text, regardless of allocation/emission order.
    private const double ChromeZ = 0;
    private const double RowZ = 1;
    private const double TextZ = 2;

    public string Name => "LiveRanking";

    public IReadOnlyDictionary<(VKind Kind, bool Interactive), int> PoolBudget { get; }

    private readonly int _maxRows;

    public LiveRankingView(int maxRows)
    {
        _maxRows = maxRows;

        PoolBudget = new Dictionary<(VKind, bool), int>
        {
            // 2 chrome quads (header bg, body bg) + 1 background quad per row.
            [(VKind.Quad, false)] = 2 + maxRows,
            // 2 chrome labels (header text, "no finishes" text) + 3 labels per row (position,
            // name, time/points).
            [(VKind.Label, false)] = 2 + maxRows * 3
        };
    }

    public VNode Render(object propsObj)
    {
        var props = (LiveRankingViewProps)propsObj;
        var settings = props.Settings;
        var width = settings.Width;

        var widgetX = settings.Position == WidgetPosition.Left
            ? -160 + WidgetPadding
            : 160 - width - WidgetPadding - 0.7;

        var children = new List<VNode>
        {
            V.Quad(key: "header-bg", size: new VVec2(width, HeaderHeight), bgColor: HeaderBg, zIndex: ChromeZ),
            V.Label(key: "header-text", position: new VVec2(width / 2, -HeaderHeight / 2 + 0.2),
                size: new VVec2(width, HeaderHeight), text: "LIVE RANKING", textColor: TextColor, textSize: 1.0,
                zIndex: TextZ, hAlign: VAlignHorizontal.Center, vAlign: VAlignVertical.Center),
            V.Quad(key: "body-bg", position: new VVec2(0, BodyOffset),
                size: new VVec2(width, Math.Max(RowHeight, _maxRows * (RowHeight + RowSpacing))), bgColor: BodyBg,
                zIndex: ChromeZ)
        };

        if (props.Scores.Count == 0)
        {
            children.Add(V.Label(key: "no-finishes", position: new VVec2(width / 2, BodyOffset - RowHeight / 2 + 0.2),
                size: new VVec2(width, RowHeight), text: "No finishes yet", textColor: TextColor, zIndex: TextZ,
                hAlign: VAlignHorizontal.Center, vAlign: VAlignVertical.Center));
        }
        else
        {
            for (var i = 0; i < props.Scores.Count; i++)
            {
                children.Add(RenderRow(props.Scores[i], i, width, props.IsPointsBased));
            }
        }

        return V.Frame(null, children, position: new VVec2(widgetX, settings.Y));
    }

    private static VNode RenderRow(LiveRankingPosition score, int index, double width, bool isPointsBased)
    {
        var y = BodyOffset - index * (RowHeight + RowSpacing);
        var timeOrPoints = isPointsBased
            ? score.Points.ToString()
            : RaceTime.FormatFromMilliseconds(score.Time);

        return V.Frame(score.AccountId, new[]
        {
            V.Quad(key: "bg", size: new VVec2(width, RowHeight), bgColor: RowBg, zIndex: RowZ),
            V.Label(key: "pos", size: new VVec2(RowHeight, RowHeight), text: (index + 1).ToString(),
                textColor: TextColor, zIndex: TextZ, hAlign: VAlignHorizontal.Center, vAlign: VAlignVertical.Center),
            V.Label(key: "name", position: new VVec2(RowHeight + 1, 0), size: new VVec2(Math.Max(1, width - RowHeight - 18), RowHeight),
                text: score.Name, textColor: TextColor, zIndex: TextZ, vAlign: VAlignVertical.Center),
            V.Label(key: "time", position: new VVec2(width - 16, 0), size: new VVec2(15, RowHeight), text: timeOrPoints,
                textColor: TextColor, zIndex: TextZ, hAlign: VAlignHorizontal.Right, vAlign: VAlignVertical.Center)
        }, position: new VVec2(0, y));
    }
}
