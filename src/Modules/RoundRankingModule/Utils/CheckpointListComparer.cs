using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Utils;

public class CheckpointListComparer(
    int startIndex = 0,
    int maxCheckpointsToLookBack = 3
) : IComparer<List<CheckpointData>>
{
    public int Compare(List<CheckpointData>? x, List<CheckpointData>? y)
    {
        if (x == null || y == null)
        {
            return 0;
        }

        var lookBackLimit = Math.Min(maxCheckpointsToLookBack, x.Count);

        for (int i = startIndex; i <= lookBackLimit; i++)
        {
            var xTime = x[^i].Time.TotalMilliseconds;
            var yTime = y[^i].Time.TotalMilliseconds;

            if (xTime == yTime)
            {
                continue;
            }

            return xTime < yTime ? -1 : 1;
        }

        return 0;
    }
}
