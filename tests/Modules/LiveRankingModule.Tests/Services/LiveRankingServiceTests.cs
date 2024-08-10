using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Config;
using EvoSC.Modules.Official.LiveRankingModule.Interfaces;
using EvoSC.Modules.Official.LiveRankingModule.Models;
using EvoSC.Modules.Official.LiveRankingModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.LiveRankingModule.Tests.Services;

public class LiveRankingServiceTests
{
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<ILiveRankingSettings> _settings = new();
    private readonly Mock<IPlayerManagerService> _playerManagerService = new();
    private readonly Mock<IMatchSettingsService> _matchSettingsService = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    private ILiveRankingService LiveRankingServiceMock()
    {
        return new LiveRankingService(
            _manialinkManager.Object,
            _server.Client.Object,
            _settings.Object,
            _playerManagerService.Object,
            _matchSettingsService.Object
        );
    }

    [Fact]
    public async Task Detects_Mode_And_Requests_Scores()
    {
        await LiveRankingServiceMock().DetectModeAndRequestScoreAsync();

        _server.Remote.Verify(
            remote => remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores"),
            Times.Once
        );
    }

    [Fact]
    public async Task Requests_Scores_When_Asked()
    {
        await LiveRankingServiceMock().RequestScoresAsync();

        _server.Remote.Verify(
            remote => remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores"),
            Times.Once
        );
    }

    [Theory]
    [InlineData(0, 0, DefaultModeScriptName.TimeAttack, 0)]
    [InlineData(0, 0, DefaultModeScriptName.Rounds, 0)]
    [InlineData(0, 10, DefaultModeScriptName.Rounds, 1)]
    [InlineData(0, 20, DefaultModeScriptName.TimeAttack, 0)]
    [InlineData(30, 0, DefaultModeScriptName.TimeAttack, 1)]
    [InlineData(40, 0, DefaultModeScriptName.Rounds, 0)]
    public async Task Maps_Scores_To_LiveRankingPositions(int bestRaceTime, int matchPoints,
        DefaultModeScriptName modeScriptName,
        int expectedLiveRankingPositionCount)
    {
        var liveRankingService = LiveRankingServiceMock();
        var player = new Player { AccountId = "UnitTest", NickName = "unit_test", UbisoftName = "unittest"};
        var scoresEventArgs = new ScoresEventArgs
        {
            Section = ModeScriptSection.Undefined,
            UseTeams = false,
            WinnerTeam = 0,
            WinnerPlayer = null,
            Teams = new List<TeamScore?>(),
            Players = new List<PlayerScore?>
            {
                new()
                {
                    AccountId = player.AccountId, 
                    Name = player.UbisoftName, 
                    BestRaceTime = bestRaceTime,
                    MatchPoints = matchPoints, 
                    Rank = 1
                }
            }
        };

        _matchSettingsService.Setup(m => m.GetCurrentModeAsync())
            .Returns(Task.FromResult(modeScriptName));

        _playerManagerService.Setup(p => p.GetPlayerAsync("UnitTest"))
            .Returns(Task.FromResult((IPlayer?)player));

        await liveRankingService.DetectModeAndRequestScoreAsync();

        if (modeScriptName == DefaultModeScriptName.TimeAttack)
        {
            Assert.False(await liveRankingService.CurrentModeIsPointsBasedAsync());
        }
        else
        {
            Assert.True(await liveRankingService.CurrentModeIsPointsBasedAsync());
        }

        if (expectedLiveRankingPositionCount > 0)
        {
            Assert.True(liveRankingService.ScoreShouldBeDisplayed(scoresEventArgs.Players.First()));
        }
        else
        {
            Assert.False(liveRankingService.ScoreShouldBeDisplayed(scoresEventArgs.Players.First()));
        }
        
        var mappedScores = await liveRankingService.MapScoresAsync(scoresEventArgs);

        Assert.IsAssignableFrom<IEnumerable<LiveRankingPosition>>(mappedScores);
    }

    [Fact]
    public async Task Maps_Scores_And_Displays_Widget()
    {
        var liveRankingService = LiveRankingServiceMock();
        var scoresEventArgs = new ScoresEventArgs
        {
            Section = ModeScriptSection.Undefined,
            UseTeams = false,
            WinnerTeam = 0,
            WinnerPlayer = null,
            Teams = new List<TeamScore?>(),
            Players = new List<PlayerScore?>()
        };

        await liveRankingService.MapScoresAndSendWidgetAsync(scoresEventArgs);

        _manialinkManager.Verify(
            m => m.SendPersistentManialinkAsync("LiveRankingModule.LiveRanking", It.IsAny<object>()),
            Times.Once
        );
    }

    [Fact]
    public Task Maps_Player_Score_To_Live_Ranking_Position()
    {
        var liveRankingService = LiveRankingServiceMock();
        var player = new Player { AccountId = "UnitTest", NickName = "unit_test", UbisoftName = "unittest"};

        _playerManagerService.Setup(s => s.GetPlayerAsync(player.AccountId))
            .Returns(Task.FromResult((IPlayer?)player));
        
        var liveRankingPosition = liveRankingService.PlayerScoreToLiveRankingPosition(new()
        {
            AccountId = player.AccountId,
            Name = player.UbisoftName,
            BestRaceTime = 1234,
            MatchPoints = 3,
            Rank = 7
        });
        
        Assert.Equal("UnitTest", liveRankingPosition.AccountId);
        Assert.Equal("unit_test", liveRankingPosition.Name);
        Assert.Equal(1234, liveRankingPosition.Time);
        Assert.Equal(3, liveRankingPosition.Points);
        Assert.Equal(7, liveRankingPosition.Position);
        
        return Task.CompletedTask;
    }

    [Fact]
    public Task Maps_Player_Score_To_Live_Ranking_Position_Where_Player_Is_Unknown()
    {
        var liveRankingService = LiveRankingServiceMock();
        var player = new Player { AccountId = "UnitTest", NickName = "unit_test", UbisoftName = "unittest"};

        _playerManagerService.Setup(s => s.GetPlayerAsync(player.AccountId))
            .Returns(Task.FromResult((IPlayer?)null));
        
        var liveRankingPosition = liveRankingService.PlayerScoreToLiveRankingPosition(new()
        {
            AccountId = player.AccountId,
            Name = player.UbisoftName,
            BestRaceTime = 1234,
            MatchPoints = 3,
            Rank = 7
        });
        
        Assert.Equal("UnitTest", liveRankingPosition.AccountId);
        Assert.Equal("unittest", liveRankingPosition.Name);
        Assert.Equal(1234, liveRankingPosition.Time);
        Assert.Equal(3, liveRankingPosition.Points);
        Assert.Equal(7, liveRankingPosition.Position);
        
        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(DefaultModeScriptName.Rounds, 0, 0, false)]
    [InlineData(DefaultModeScriptName.Rounds, 0, 1, true)]
    [InlineData(DefaultModeScriptName.Rounds, 1, 0, false)]
    [InlineData(DefaultModeScriptName.TimeAttack, 0, 0, false)]
    [InlineData(DefaultModeScriptName.TimeAttack, 0, 1, false)]
    [InlineData(DefaultModeScriptName.TimeAttack, 1, 0, true)]
    public Task Determines_Whether_Score_Should_Be_Send_To_Widget(DefaultModeScriptName modeScriptName, int time, int points, bool expected)
    {
        var liveRankingService = LiveRankingServiceMock();
        
        _matchSettingsService.Setup(s => s.GetCurrentModeAsync())
            .Returns(Task.FromResult(modeScriptName));

        liveRankingService.DetectModeAndRequestScoreAsync();

        var shouldBeDisplayed = liveRankingService.ScoreShouldBeDisplayed(new PlayerScore
        {
            BestRaceTime = time,
            MatchPoints = points
        });
        
        Assert.Equal(expected, shouldBeDisplayed);
        
        return Task.CompletedTask;
    }
}
