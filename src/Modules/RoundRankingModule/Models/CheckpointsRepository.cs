using System.Collections.Concurrent;
using EvoSC.Modules.Official.RoundRankingModule.Utils;

namespace EvoSC.Modules.Official.RoundRankingModule.Models;

/// <summary>
/// CheckpointsRepository contains all <seealso cref="CheckpointData"/> for the ongoing round,
/// where the key of the dictionary is the players account-ID.
/// </summary>
public class CheckpointsRepository(int maxLookBack = 3) : ConcurrentDictionary<string, List<CheckpointData>>
{
    /// <summary>
    /// Sorts and returns the contents of the dictionary by the checkpoint progression and times at each checkpoint.
    /// </summary>
    /// <returns></returns>
    public List<CheckpointData> GetSortedData()
    {
        return this.Values
            .OrderByDescending(cpData => cpData.Last().CheckpointId)
            .ThenBy(cpDataList => cpDataList, new CheckpointListTimesComparer(maxLookBack))
            .Select(cpDataList => cpDataList.Last())
            .ToList();
    }

    /// <summary>
    /// Adds a checkpoint data to the players list.
    /// Capped at 3 entries.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="checkpointData"></param>
    public void AddCheckpoint(string accountId, CheckpointData checkpointData)
    {
        List<CheckpointData> playerCheckpoints = GetOrAdd(accountId, (k) => []);

        lock (playerCheckpoints)
        {
            playerCheckpoints.Add(checkpointData);
            playerCheckpoints.Sort((a, b) => a.CheckpointId.CompareTo(b.CheckpointId));

            if (playerCheckpoints.Count <= maxLookBack)
            {
                return;
            }

            playerCheckpoints.RemoveRange(0, 1);
        }
    }

    /// <summary>
    /// Gets the latest checkpoints of the player.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public List<CheckpointData> GetCheckpoints(string accountId)
    {
        if (!TryGetValue(accountId, out var list))
        {
            return [];
        }

        lock (list)
        {
            return list.ToList();
        }
    }
}
