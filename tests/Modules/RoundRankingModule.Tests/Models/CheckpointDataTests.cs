using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RoundRankingModule.Models;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Models;

public class CheckpointDataTests
{
    [Theory]
    [InlineData("*fakeplayer1*", 1, 0, false)]
    [InlineData("*fakeplayer2*", 2, 10, false)]
    [InlineData("*fakeplayer3*", 3, 100, false)]
    [InlineData("*fakeplayer4*", 4, 1000, true)]
    public void Converts_Waypoint_Event_Args_To_Checkpoint_Data(string accountId, int cpId, int time,
        bool isEndRace)
    {
        var player = new OnlinePlayer { State = PlayerState.Playing, AccountId = accountId };

        var waypointEventArgs = new WayPointEventArgs
        {
            Login = player.GetLogin(),
            AccountId = player.AccountId,
            RaceTime = time,
            LapTime = time,
            CheckpointInRace = cpId,
            CheckpointInLap = cpId,
            IsEndRace = isEndRace,
            IsEndLap = false,
            CurrentRaceCheckpoints = [],
            CurrentLapCheckpoints = [],
            BlockId = "",
            Speed = 0,
            Time = 0
        };

        var checkpointData = CheckpointData.FromWaypointEventArgs(player, waypointEventArgs);

        Assert.Equal(player, checkpointData.Player);
        Assert.Equal(time, checkpointData.Time.TotalMilliseconds);
        Assert.Equal(cpId, checkpointData.CheckpointId);
        Assert.False(checkpointData.IsFinish);
        Assert.False(checkpointData.IsDNF);
    }

    [Fact]
    public void Creates_DNF_Entry()
    {
        var player = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" };
        var checkpointData = CheckpointData.CreateDnfEntry(player);

        Assert.Equal(player, checkpointData.Player);
        Assert.Equal(0, checkpointData.Time.TotalMilliseconds);
        Assert.Equal(-1, checkpointData.CheckpointId);
        Assert.False(checkpointData.IsFinish);
        Assert.True(checkpointData.IsDNF);
    }

    [Theory]
    [InlineData(1000, 1234, 234)]
    [InlineData(1234, 1000, 234)]
    [InlineData(-100, 100, 200)]
    [InlineData(-100, -20, 80)]
    public void Calculates_Absolute_Time_Difference(int time1, int time2, int expectedDifference)
    {
        var cpData1 = new CheckpointData
        {
            Player = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" },
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(time1),
            IsFinish = false,
            IsDNF = false
        };
        var cpData2 = new CheckpointData
        {
            Player = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer2*" },
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(time2),
            IsFinish = false,
            IsDNF = false
        };

        Assert.Equal(expectedDifference, cpData1.GetTimeDifferenceAbsolute(cpData2).TotalMilliseconds);
    }

    [Theory]
    [InlineData(0, 1234, false, false, "1")]
    [InlineData(2, 1234, false, false, "3")]
    [InlineData(4, 1234, true, false, "")] //GameIcons.Icons.FlagO
    [InlineData(6, 1234, true, true, "")] //GameIcons.Icons.FlagO
    [InlineData(8, 1234, false, true, "")] //GameIcons.Icons.FlagCheckered
    public void Return_Correct_Index_Text(int checkpointId, int time, bool isDnf, bool isFinish,
        string expectedText)
    {
        var cpData = new CheckpointData
        {
            Player = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" },
            CheckpointId = checkpointId,
            Time = RaceTime.FromMilliseconds(time),
            IsFinish = isFinish,
            IsDNF = isDnf
        };

        Assert.Equal(expectedText, cpData.IndexText());
    }

    [Theory]
    [InlineData(500, null, false, ".500")]
    [InlineData(1000, null, false, "1.000")]
    [InlineData(10_123, null, false, "10.123")]
    [InlineData(0, null, true, "DNF")]
    [InlineData(1000, 500, false, ".500")]
    [InlineData(1234, 999, false, ".999")]
    public void Formats_Time_Correctly(int time, int? timeDifference, bool isDnf, string expectedText)
    {
        var cpData = new CheckpointData
        {
            Player = new OnlinePlayer { State = PlayerState.Playing, AccountId = "*fakeplayer1*" },
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(time),
            TimeDifference = timeDifference != null ? RaceTime.FromMilliseconds((int)timeDifference) : null,
            IsFinish = false,
            IsDNF = isDnf
        };

        Assert.Equal(expectedText, cpData.FormattedTime());
    }
}
