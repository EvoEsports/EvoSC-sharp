using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Models;

public class CheckpointsRepositorySortingTests
{
    private readonly CheckpointsRepository _checkpointsRepository = new(3);

    private readonly Dictionary<string, List<dynamic>> _data = new()
    {
        ["*fakeplayer1*"] =
        [
            new { checkpointIndex = 1, time = 1 },
            new { checkpointIndex = 2, time = 55 },
            new { checkpointIndex = 3, time = 99 },
            new { checkpointIndex = 4, time = 500 },
        ],
        ["*fakeplayer2*"] =
        [
            new { checkpointIndex = 1, time = 1 },
            new { checkpointIndex = 2, time = 11 },
            new { checkpointIndex = 3, time = 99 },
            new { checkpointIndex = 4, time = 500 },
        ],
        ["*fakeplayer3*"] =
        [
            new { checkpointIndex = 1, time = 1 },
            new { checkpointIndex = 2, time = 10 },
            new { checkpointIndex = 3, time = 140 },
            new { checkpointIndex = 4, time = 1200 },
        ],
        ["*fakeplayer4*"] =
        [
            new { checkpointIndex = 1, time = 1 },
            new { checkpointIndex = 2, time = 99 },
            new { checkpointIndex = 3, time = 130 },
            new { checkpointIndex = 4, time = 1200 },
        ],
        ["*fakeplayer5*"] =
        [
            new { checkpointIndex = 1, time = 1 },
            new { checkpointIndex = 2, time = 20 },
            new { checkpointIndex = 3, time = 120 },
            new { checkpointIndex = 4, time = 1400 },
        ],
        ["*fakeplayer6*"] =
        [
            new { checkpointIndex = 1, time = 1 },
            new { checkpointIndex = 2, time = 10 },
            new { checkpointIndex = 3, time = 110 },
            new { checkpointIndex = -1, time = 200 },
        ],
        ["*fakeplayer7*"] =
        [
            new { checkpointIndex = 1, time = 1 },
            new { checkpointIndex = 2, time = 10 },
            new { checkpointIndex = 3, time = 110 },
            new { checkpointIndex = -1, time = 250 },
        ],
    };

    public CheckpointsRepositorySortingTests()
    {
        foreach (var (accountId, checkpoints) in _data)
        {
            foreach (var data in checkpoints)
            {
                _checkpointsRepository.AddCheckpoint(accountId,
                    CreateFakeCheckpointData(accountId, data.checkpointIndex, data.time));
            }
        }
    }

    [Theory]
    [InlineData(0, "*fakeplayer2*", 500)]
    [InlineData(1, "*fakeplayer1*", 500)]
    [InlineData(2, "*fakeplayer4*", 1200)]
    [InlineData(3, "*fakeplayer3*", 1200)]
    [InlineData(4, "*fakeplayer5*", 1400)]
    [InlineData(5, "*fakeplayer6*", 200)]
    [InlineData(6, "*fakeplayer7*", 250)]
    public void Sorts_Entries_Correctly(int index, string expectedAccountId, int expectedTime)
    {
        var sorted = _checkpointsRepository.GetSortedData();

        Assert.Equal(expectedAccountId, sorted[index].Player.AccountId);
        Assert.Equal(expectedTime, sorted[index].Time.TotalMilliseconds);
    }

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
}
