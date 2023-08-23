using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Utils;

public class RankingComparer : IEqualityComparer<ExpandedLiveRankingPosition>
{
    public bool Equals(ExpandedLiveRankingPosition? x, ExpandedLiveRankingPosition? y)
    {
        return x?.player == y?.player;
    }

    public int GetHashCode(ExpandedLiveRankingPosition obj)
    {
        return obj.GetHashCode();
    }
}
