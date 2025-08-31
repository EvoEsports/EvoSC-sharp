namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class PlayerCheckpointsData
{
    public readonly List<CheckpointData> Checkpoints = [];
    public readonly object Lock = new();
}
