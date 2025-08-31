using System.Collections.Concurrent;
using EvoSC.Modules.Official.RoundRankingModule.Utils;
using LinqToDB.Common;

namespace EvoSC.Modules.Official.RoundRankingModule.Models;

/// <summary>
/// CheckpointsRepository contains all <seealso cref="CheckpointData"/> for the ongoing round,
/// where the key of the dictionary is the players account-ID.
/// </summary>
public class CheckpointsRepository(int maxLookBack = 3)
{
    private readonly ConcurrentDictionary<string, List<CheckpointData>> _playerCheckpoints = new();
    private readonly object _lock = new();

    /// <summary>
    /// Sorts and returns the contents of the dictionary by the checkpoint progression and times at each checkpoint.
    /// </summary>
    /// <returns></returns>
    public List<CheckpointData> GetSortedData()
    {
        List<List<CheckpointData>> checkpoints;

        lock (_lock)
        {
            checkpoints = _playerCheckpoints.Values.ToList();
        }

        return checkpoints
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
        lock (_lock)
        {
            List<CheckpointData> playerCheckpoints = _playerCheckpoints.GetOrAdd(accountId, (v) => []);

            if (checkpointData.IsDNF)
            {
                playerCheckpoints.Clear();
            }

            playerCheckpoints.Add(checkpointData);
            playerCheckpoints.Sort((x, y) => x.CheckpointId.CompareTo(y.CheckpointId));

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
        lock (_lock)
        {
            if (_playerCheckpoints.TryGetValue(accountId, out List<CheckpointData>? checkpointList))
            {
                return checkpointList.ToList();
            }
        }

        return [];
    }

    /// <summary>
    /// Removes all entries from the repository.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _playerCheckpoints.Clear();
        }
    }

    /// <summary>
    /// Checks whether the repository contains any values.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        lock (_lock)
        {
            return _playerCheckpoints.IsNullOrEmpty();
        }
    }

    /// <summary>
    /// Removes all checkpoints of that player.
    /// </summary>
    /// <param name="accountId"></param>
    public void Remove(string accountId)
    {
        lock (_lock)
        {
            _playerCheckpoints.TryRemove(accountId, out _);
        }
    }
}
