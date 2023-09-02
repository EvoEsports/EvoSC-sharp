using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public record ExpandedLiveRankingPosition
{
    public required IOnlinePlayer player { get; init; }
    public required int cpTime { get; init; }
    public required int cpIndex { get; init; }
    public required bool isDNF { get; init; }
    public required bool isFinish { get; init; }
    public int diffToFirst { get; set; }
}
