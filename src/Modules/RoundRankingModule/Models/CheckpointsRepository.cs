namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class CheckpointsRepository : Dictionary<int, List<CheckpointData>>
{
    public int TotalEntries()
    {
        return this.Values.Select(cpGroup => cpGroup.Count()).Sum();
    }

    public bool CanCollectAnotherPlayer(int maxRows)
    {
        return TotalEntries() < maxRows;
    }

    public List<CheckpointData> CreateIndexIfMissing(int index)
    {
        if (this.ContainsKey(index))
        {
            return this[index];
        }

        this[index] = [];

        return this[index];
    }

    public void AddAndSort(CheckpointData data)
    {
        var checkpointGroup = CreateIndexIfMissing(data.CheckpointId);
        checkpointGroup.Add(data);
        this[data.CheckpointId] = checkpointGroup.OrderBy(checkpoint => checkpoint.Time.TotalMilliseconds).ToList();
    }

    public List<CheckpointData> GetBestTimes(int maxRows)
    {
        var bestTimes = new List<CheckpointData>();
        var reversedCheckpointRepository = this.OrderByDescending(pair => pair.Key).ToDictionary();

        foreach (var checkpointGroup in reversedCheckpointRepository.Values)
        {
            foreach (CheckpointData data in checkpointGroup)
            {
                bestTimes.Add(data);
                if (bestTimes.Count >= maxRows)
                {
                    break;
                }
            }

            if (bestTimes.Count >= maxRows)
            {
                break;
            }
        }

        return bestTimes;
    }
}
