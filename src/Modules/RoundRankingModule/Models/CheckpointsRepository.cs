using System.Collections.Concurrent;

namespace EvoSC.Modules.Official.RoundRankingModule.Models;

/// <summary>
/// CheckpointsRepository contains all <seealso cref="CheckpointData"/> for the ongoing round,
/// where the key of the dictionary is the players account-ID.
/// </summary>
public class CheckpointsRepository : ConcurrentDictionary<string, CheckpointData>
{
    /// <summary>
    /// Sorts and returns the contents of the dictionary by the checkpoint progression and times at each checkpoint.
    /// </summary>
    /// <returns></returns>
    public List<CheckpointData> GetSortedData()
    {
        return this.Values
            .OrderByDescending(cpData => cpData.CheckpointId)
            .ThenBy(cpData => cpData.Time.TotalMilliseconds)
            .ToList();
    }
}
