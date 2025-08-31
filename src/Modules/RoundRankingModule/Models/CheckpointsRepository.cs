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
    private readonly ConcurrentDictionary<string, PlayerCheckpointsData> _playerCheckpoints = new();
    private readonly object _lock = new();

    /// <summary>
    /// Sorts and returns the contents of the dictionary by the checkpoint progression and times at each checkpoint.
    /// </summary>
    /// <returns></returns>
    public List<CheckpointData> GetSortedData()
    {
        return _playerCheckpoints.Values
            .Select(playerCheckpointsData => playerCheckpointsData.Checkpoints)
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
        PlayerCheckpointsData playerCheckpointsData =
            _playerCheckpoints.GetOrAdd(accountId, new PlayerCheckpointsData());

        lock (playerCheckpointsData.Lock)
        {
            if (checkpointData.IsDNF)
            {
                playerCheckpointsData.Checkpoints.Clear();
            }

            playerCheckpointsData.Checkpoints.Add(checkpointData);
            playerCheckpointsData.Checkpoints.Sort((x, y) => x.CheckpointId.CompareTo(y.CheckpointId));

            if (playerCheckpointsData.Checkpoints.Count <= maxLookBack)
            {
                return;
            }

            playerCheckpointsData.Checkpoints.RemoveRange(0, 1);
        }
    }

    /// <summary>
    /// Gets the latest checkpoints of the player.
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public List<CheckpointData> GetCheckpoints(string accountId)
    {
        if (!_playerCheckpoints.TryGetValue(accountId, out PlayerCheckpointsData? playerCheckpointsData))
        {
            return [];
        }

        lock (playerCheckpointsData.Lock)
        {
            return playerCheckpointsData.Checkpoints.ToList();
        }
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
            return _playerCheckpoints.IsEmpty;
        }
    }

    /// <summary>
    /// Removes all checkpoints of that player.
    /// </summary>
    /// <param name="accountId"></param>
    public void Remove(string accountId)
    {
        if (!_playerCheckpoints.TryGetValue(accountId, out PlayerCheckpointsData? playerCheckpointsData))
        {
            return;
        }

        lock (playerCheckpointsData.Lock)
        {
            _playerCheckpoints.TryRemove(accountId, out _);
        }
    }
}
