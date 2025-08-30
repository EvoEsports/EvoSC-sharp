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
            ["*fakeplayer1*"] =
            [
                CreateFakeCheckpointData("*fakeplayer1*", 2, 55),
                CreateFakeCheckpointData("*fakeplayer1*", 2, 99),
                CreateFakeCheckpointData("*fakeplayer1*", 2, 500)
            ], // Expected placement: 2.
            ["*fakeplayer2*"] =
            [
                CreateFakeCheckpointData("*fakeplayer2*", 2, 11),
                CreateFakeCheckpointData("*fakeplayer2*", 2, 99),
                CreateFakeCheckpointData("*fakeplayer2*", 2, 500)
            ], // Expected placement: 1.
            ["*fakeplayer3*"] =
            [
                CreateFakeCheckpointData("*fakeplayer3*", 1, 10),
                CreateFakeCheckpointData("*fakeplayer3*", 1, 140),
                CreateFakeCheckpointData("*fakeplayer3*", 1, 1200)
            ], // Expected placement: 4.
            ["*fakeplayer4*"] =
            [
                CreateFakeCheckpointData("*fakeplayer4*", 1, 99),
                CreateFakeCheckpointData("*fakeplayer4*", 1, 130),
                CreateFakeCheckpointData("*fakeplayer4*", 1, 1200)
            ], // Expected placement: 3.
            ["*fakeplayer5*"] =
            [
                CreateFakeCheckpointData("*fakeplayer5*", 1, 20),
                CreateFakeCheckpointData("*fakeplayer5*", 1, 120),
                CreateFakeCheckpointData("*fakeplayer5*", 1, 1400)
            ], // Expected placement: 5.
            ["*fakeplayer6*"] =
            [
                CreateFakeCheckpointData("*fakeplayer6*", -1, 10),
                CreateFakeCheckpointData("*fakeplayer6*", -1, 110),
                CreateFakeCheckpointData("*fakeplayer6*", -1, 2000)
            ], // Expected placement: 6.
            ["*fakeplayer7*"] =
            [
                CreateFakeCheckpointData("*fakeplayer7*", -1, 10),
                CreateFakeCheckpointData("*fakeplayer7*", -1, 110),
                CreateFakeCheckpointData("*fakeplayer7*", -1, 2500)
            ], // Expected placement: 7.
        };

        var sorted = cpRepository.GetSortedData();

        Assert.Equal("*fakeplayer2*", sorted[0].Player.AccountId);
        Assert.Equal(500, sorted[0].Time.TotalMilliseconds);

        Assert.Equal("*fakeplayer1*", sorted[1].Player.AccountId);
        Assert.Equal(500, sorted[1].Time.TotalMilliseconds);

        Assert.Equal("*fakeplayer4*", sorted[2].Player.AccountId);
        Assert.Equal(1200, sorted[2].Time.TotalMilliseconds);

        Assert.Equal("*fakeplayer3*", sorted[3].Player.AccountId);
        Assert.Equal(1200, sorted[3].Time.TotalMilliseconds);

        Assert.Equal("*fakeplayer5*", sorted[4].Player.AccountId);
        Assert.Equal(1400, sorted[4].Time.TotalMilliseconds);

        Assert.Equal("*fakeplayer6*", sorted[5].Player.AccountId);
        Assert.Equal(2000, sorted[5].Time.TotalMilliseconds);

        Assert.Equal("*fakeplayer7*", sorted[6].Player.AccountId);
        Assert.Equal(2500, sorted[6].Time.TotalMilliseconds);
    }

    [Fact]
    public void Keeps_Latest_Checkpoints_Only()
    {
        var checkpointRepository = new CheckpointsRepository(2);

        checkpointRepository.AddCheckpoint("unittest", CreateFakeCheckpointData("unittest", 4, 4000));
        checkpointRepository.AddCheckpoint("unittest", CreateFakeCheckpointData("unittest", 3, 3000));
        checkpointRepository.AddCheckpoint("unittest", CreateFakeCheckpointData("unittest", 2, 2000));
        checkpointRepository.AddCheckpoint("unittest", CreateFakeCheckpointData("unittest", 1, 1000));

        var checkpointList = checkpointRepository.GetCheckpoints("unittest");

        Assert.Equal(2, checkpointList.Count);
        Assert.Equal(3, checkpointList.First().CheckpointId);
        Assert.Equal(3000, checkpointList.First().Time.TotalMilliseconds);
        Assert.Equal(4, checkpointList.Last().CheckpointId);
        Assert.Equal(4000, checkpointList.Last().Time.TotalMilliseconds);
    }
}
