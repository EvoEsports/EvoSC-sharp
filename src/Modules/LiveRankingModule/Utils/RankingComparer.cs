﻿using EvoSC.Modules.Official.LiveRankingModule.Models;

namespace EvoSC.Modules.Official.LiveRankingModule.Utils;

public class RankingComparer : IEqualityComparer<LiveRankingWidgetPosition>
{
    public bool Equals(LiveRankingWidgetPosition? x, LiveRankingWidgetPosition? y)
    {
        if (x == null || y == null)
        {
            return false;
        }
        
        return x.Player.AccountId == y.Player.AccountId;
    }

    public int GetHashCode(LiveRankingWidgetPosition obj)
    {
        return obj.GetHashCode();
    }
}
