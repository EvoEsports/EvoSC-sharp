using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Utils;

public class CheckpointListTimesComparer(int maxCheckpointsToLookBack = 3) : IComparer<List<CheckpointData>>
{
    public int Compare(List<CheckpointData>? x, List<CheckpointData>? y)
    {
        if (x == null || y == null)
        {
            return 0;
        }

        var lookBackLimit = Math.Min(maxCheckpointsToLookBack, x.Count);

        for (int i = 1; i <= lookBackLimit; i++)
        {
            var xTime = x[^i].Time.TotalMilliseconds;
            var yTime = y[^i].Time.TotalMilliseconds;
            var comparisonResult = xTime.CompareTo(yTime);

            if (comparisonResult != 0)
            {
                return comparisonResult;
            }
        }

        return 0;
    }
}
