namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class CheckpointsRepository : Dictionary<string, CheckpointData>
{
    public List<CheckpointData> GetBestTimes(int maxRows)
    {
        return this.Values.OrderBy(cpData => cpData.Time.TotalMilliseconds)
            .ThenByDescending(cpData => cpData.CheckpointId)
            .ThenBy(cpData => cpData.IsDNF)
            .Take(maxRows)
            .ToList();
    }
}
