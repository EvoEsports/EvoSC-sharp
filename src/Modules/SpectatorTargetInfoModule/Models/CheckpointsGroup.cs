using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;

public class CheckpointsGroup : List<CheckpointData>
{
    public CheckpointsGroup() { }

    public CheckpointsGroup(IEnumerable<CheckpointData> checkpoints)
    {
        this.AddRange(checkpoints);
    }

    public CheckpointsGroup ToSortedGroup()
    {
        return new CheckpointsGroup(this.OrderBy(cpData => cpData.time));
    }

    public CheckpointData? GetPlayer(string playerLogin)
    {
        return this.FirstOrDefault(cpData => cpData.player.GetLogin() == playerLogin);
    }

    public int GetRank(string playerLogin)
    {
        var rank = 1;
        foreach (var checkpointData in this)
        {
            if (checkpointData.player.GetLogin() == playerLogin) return rank;
            rank++;
        }

        return rank;
    }
}
