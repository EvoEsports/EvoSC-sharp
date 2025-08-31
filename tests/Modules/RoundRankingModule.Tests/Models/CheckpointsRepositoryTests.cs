using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Models;

public class CheckpointsRepositoryTests
{
    [Fact]
    public void Keeps_Latest_Checkpoints_Only()
    {
        var checkpointRepository = new CheckpointsRepository(2);

        for (int i = 4; i > 0; i--)
        {
            checkpointRepository.AddCheckpoint("unittest",
                new CheckpointData
                {
                    Player = new OnlinePlayer { State = PlayerState.Playing, AccountId = "unittest" },
                    CheckpointId = i,
                    Time = RaceTime.FromMilliseconds(i * 1000),
                    IsFinish = false,
                    IsDNF = false
                });
        }

        var checkpointList = checkpointRepository.GetCheckpoints("unittest");

        Assert.Equal(2, checkpointList.Count);
        Assert.Equal(3, checkpointList.First().CheckpointId);
        Assert.Equal(3000, checkpointList.First().Time.TotalMilliseconds);
        Assert.Equal(4, checkpointList.Last().CheckpointId);
        Assert.Equal(4000, checkpointList.Last().Time.TotalMilliseconds);
    }
}
