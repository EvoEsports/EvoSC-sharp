using EvoSC.Modules.Official.LiveRankingModule.Config;
using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Vdom;

public sealed record LiveRankingViewProps(
    ILiveRankingSettings Settings,
    bool IsPointsBased,
    IReadOnlyList<LiveRankingPosition> Scores);
