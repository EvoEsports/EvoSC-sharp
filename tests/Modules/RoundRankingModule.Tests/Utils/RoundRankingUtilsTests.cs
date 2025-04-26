using System.Collections.Concurrent;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.RoundRankingModule.Models;
using EvoSC.Modules.Official.RoundRankingModule.Utils;

namespace EvoSC.Modules.Official.RoundRankingModule.Tests.Utils;

public class RoundRankingUtilsTests
{
    private static CheckpointData CreateFakeCheckpointData(bool isFinished, int gainedPoints = 0,
        PlayerTeam team = PlayerTeam.Unknown, int time = 0)
    {
        return new CheckpointData
        {
            Player = new OnlinePlayer { State = PlayerState.Playing, Team = team },
            CheckpointId = 0,
            Time = RaceTime.FromMilliseconds(time),
            IsFinish = isFinished,
            GainedPoints = gainedPoints,
            IsDNF = false
        };
    }

    [Fact]
    public void Detects_Players_In_Finish()
    {
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(false),
            CreateFakeCheckpointData(true),
            CreateFakeCheckpointData(false),
            CreateFakeCheckpointData(false),
        };

        Assert.True(RoundRankingUtils.HasPlayerInFinish(checkpoints));
    }

    [Fact]
    public void Does_Not_Detect_Players_In_Finish_When_None_Finished()
    {
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(false),
            CreateFakeCheckpointData(false),
            CreateFakeCheckpointData(false),
            CreateFakeCheckpointData(false),
        };

        Assert.False(RoundRankingUtils.HasPlayerInFinish(checkpoints));
    }

    [Fact]
    public void Gets_Winner_Team_One()
    {
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(true, gainedPoints: 10, team: PlayerTeam.Team1),
            CreateFakeCheckpointData(true, gainedPoints: 6, team: PlayerTeam.Team2),
            CreateFakeCheckpointData(true, gainedPoints: 4, team: PlayerTeam.Team1),
            CreateFakeCheckpointData(true, gainedPoints: 3, team: PlayerTeam.Team2)
        };

        Assert.Equal(PlayerTeam.Team1, RoundRankingUtils.GetWinnerTeam(checkpoints));
    }

    [Fact]
    public void Gets_Winner_Team_Two()
    {
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(true, gainedPoints: 10, team: PlayerTeam.Team2),
            CreateFakeCheckpointData(true, gainedPoints: 6, team: PlayerTeam.Team1),
            CreateFakeCheckpointData(true, gainedPoints: 4, team: PlayerTeam.Team2),
            CreateFakeCheckpointData(true, gainedPoints: 3, team: PlayerTeam.Team1)
        };

        Assert.Equal(PlayerTeam.Team2, RoundRankingUtils.GetWinnerTeam(checkpoints));
    }

    [Fact]
    public void Gets_Winner_Team_Draw()
    {
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(true, gainedPoints: 10, team: PlayerTeam.Team2),
            CreateFakeCheckpointData(true, gainedPoints: 6, team: PlayerTeam.Team1),
            CreateFakeCheckpointData(true, gainedPoints: 4, team: PlayerTeam.Team1)
        };

        Assert.Equal(PlayerTeam.Unknown, RoundRankingUtils.GetWinnerTeam(checkpoints));
    }

    [Fact]
    public void Calculates_And_Sets_Time_Differences()
    {
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(true, time: 1000),
            CreateFakeCheckpointData(true, time: 1234),
            CreateFakeCheckpointData(true, time: 4321)
        };

        RoundRankingUtils.CalculateAndSetTimeDifferenceOnResult(checkpoints);

        Assert.Null(checkpoints[0].TimeDifference);
        Assert.Equal(234, checkpoints[1].TimeDifference?.TotalMilliseconds);
        Assert.Equal(3321, checkpoints[2].TimeDifference?.TotalMilliseconds);
    }

    [Fact]
    public void Does_Not_Calculate_Time_Differences_If_No_Entries()
    {
        var checkpoints = new List<CheckpointData>();

        RoundRankingUtils.CalculateAndSetTimeDifferenceOnResult(checkpoints);

        Assert.Empty(checkpoints);
    }

    [Fact]
    public void Does_Not_Calculate_Time_Differences_If_Less_Than_Two_Entries()
    {
        var checkpoints = new List<CheckpointData> { CreateFakeCheckpointData(true, time: 1000), };

        RoundRankingUtils.CalculateAndSetTimeDifferenceOnResult(checkpoints);

        Assert.Null(checkpoints[0].TimeDifference);
    }

    [Fact]
    public void Applies_Correct_Team_Colors_As_Accent()
    {
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(true, gainedPoints: 10, team: PlayerTeam.Team2),
            CreateFakeCheckpointData(true, gainedPoints: 6, team: PlayerTeam.Team1),
            CreateFakeCheckpointData(true, gainedPoints: 4, team: PlayerTeam.Team1)
        };

        var teamColors = new ConcurrentDictionary<PlayerTeam, string>
        {
            [PlayerTeam.Unknown] = "000000", [PlayerTeam.Team1] = "ff0055", [PlayerTeam.Team2] = "00ff00",
        };

        RoundRankingUtils.ApplyTeamColorsAsAccentColors(checkpoints, teamColors);

        Assert.Equal("00ff00", checkpoints[0].AccentColor);
        Assert.Equal("ff0055", checkpoints[1].AccentColor);
        Assert.Equal("ff0055", checkpoints[2].AccentColor);
    }

    [Fact]
    public void Sets_Gained_Points_On_Result()
    {
        const string AccentColor = "123123";
        var pointsRepartition = new PointsRepartition("10,6,4,3,2,1");
        var checkpoints = new List<CheckpointData>
        {
            CreateFakeCheckpointData(true, time: 1000, team: PlayerTeam.Team2),
            CreateFakeCheckpointData(true, time: 1234, team: PlayerTeam.Team1),
            CreateFakeCheckpointData(false, time: 0, team: PlayerTeam.Team1),
            CreateFakeCheckpointData(true, time: 2345, team: PlayerTeam.Team2),
        };

        RoundRankingUtils.SetGainedPointsOnResult(checkpoints, pointsRepartition, AccentColor);

        Assert.Equal(AccentColor, checkpoints[0].AccentColor);
        Assert.Equal(10, checkpoints[0].GainedPoints);
        
        Assert.Equal(AccentColor, checkpoints[1].AccentColor);
        Assert.Equal(6, checkpoints[1].GainedPoints);
        
        Assert.Null(checkpoints[2].AccentColor);
        Assert.Equal(0, checkpoints[2].GainedPoints);
        
        Assert.Equal(AccentColor, checkpoints[3].AccentColor);
        Assert.Equal(4, checkpoints[3].GainedPoints);
    }
}
