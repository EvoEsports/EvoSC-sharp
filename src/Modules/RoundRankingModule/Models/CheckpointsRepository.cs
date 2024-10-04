namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class CheckpointsRepository : Dictionary<int, List<CheckpointData>>
{
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
        this[data.CheckpointId].Add(data);
        this[data.CheckpointId] = this[data.CheckpointId].OrderBy(checkpoint => checkpoint.Time.TotalMilliseconds).ToList();
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
