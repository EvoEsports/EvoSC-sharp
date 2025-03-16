using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Models;

public class CheckpointsRepositoryTests
{
    private static CheckpointData CreateFakeCheckpointData(string accountId, int cpId, int time)
    {
        return new CheckpointData
        {
            Player = new OnlinePlayer { State = PlayerState.Playing, AccountId = accountId },
            CheckpointId = cpId,
            Time = RaceTime.FromMilliseconds(time),
            IsFinish = false,
            IsDNF = cpId == -1
        };
    }

    [Fact]
    public void Sorts_Entries_Correctly()
    {
        var cpRepository = new CheckpointsRepository
        {
            ["*fakeplayer1*"] = CreateFakeCheckpointData("*fakeplayer1*", 2, 500), // 2.
            ["*fakeplayer2*"] = CreateFakeCheckpointData("*fakeplayer2*", 2, 250), // 1.
            ["*fakeplayer3*"] = CreateFakeCheckpointData("*fakeplayer3*", 1, 1200), // 4.
            ["*fakeplayer4*"] = CreateFakeCheckpointData("*fakeplayer4*", 1, 1000), // 3.
            ["*fakeplayer5*"] = CreateFakeCheckpointData("*fakeplayer5*", 1, 1400), // 5.
            ["*fakeplayer6*"] = CreateFakeCheckpointData("*fakeplayer6*", -1, 2000), // 6.
            ["*fakeplayer7*"] = CreateFakeCheckpointData("*fakeplayer7*", -1, 2500), // 7.
        };

        var sorted = cpRepository.GetSortedData();
        Assert.Equal("*fakeplayer1*", sorted[1].Player.AccountId);
        Assert.Equal("*fakeplayer2*", sorted[0].Player.AccountId);
        Assert.Equal("*fakeplayer3*", sorted[3].Player.AccountId);
        Assert.Equal("*fakeplayer4*", sorted[2].Player.AccountId);
        Assert.Equal("*fakeplayer5*", sorted[4].Player.AccountId);
        Assert.Equal("*fakeplayer6*", sorted[5].Player.AccountId);
        Assert.Equal("*fakeplayer7*", sorted[6].Player.AccountId);
    }
}
