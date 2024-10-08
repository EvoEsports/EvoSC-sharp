using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Models;

public class CheckpointsGroup : List<CheckpointData>
{
    public CheckpointData? GetPlayerCheckpointData(string playerLogin)
    {
        return this.Find(cpData => cpData.player.GetLogin() == playerLogin);
    }

    public int GetRank(string playerLogin)
    {
        var rank = 1;
        foreach (var checkpointData in this)
        {
            if (checkpointData.player.GetLogin() == playerLogin)
            {
                return rank;
            }

            rank++;
        }

        return rank;
    }

    public bool ForgetPlayer(string playerLogin) =>
        (from checkpointData in this
            where checkpointData.player.GetLogin() == playerLogin
            select this.Remove(checkpointData)
        ).FirstOrDefault();
}
