using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public record ExpandedLiveRankingPosition(IOnlinePlayer player, int cpTime, int cpIndex, bool isDNF, bool isFinish);
