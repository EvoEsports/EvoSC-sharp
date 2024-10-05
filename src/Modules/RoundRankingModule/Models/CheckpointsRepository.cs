namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class CheckpointsRepository : Dictionary<string, CheckpointData>
{
    public List<CheckpointData> GetBestTimes(int maxRows)
    {
        return this.Values.OrderBy(cpData => cpData.IsDNF)
            .ThenByDescending(cpData => cpData.CheckpointId)
            .ThenBy(cpData => cpData.Time.TotalMilliseconds)
            .Take(maxRows)
            .ToList();
    }
}
