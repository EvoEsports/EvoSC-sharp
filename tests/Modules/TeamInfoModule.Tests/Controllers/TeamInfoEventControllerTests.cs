using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.TeamInfoModule.Controllers;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamInfoModule.Tests.Controllers;

public class TeamInfoEventControllerTests : ControllerMock<TeamInfoEventController, IEventControllerContext>
{
    private Mock<ITeamInfoService> _teamInfoService = new();

    public TeamInfoEventControllerTests()
    {
        InitMock(_teamInfoService);
    }

    [Fact]
    public async Task Sets_Team_Mode_Disabled_If_UseTeams_Is_False()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeams())
            .Returns(Task.FromResult(true));
        
        await Controller.OnScores(null,
            new ScoresEventArgs
            {
                Section = ModeScriptSection.Undefined,
                UseTeams = false,
                WinnerTeam = 0,
                WinnerPlayer = null,
                Teams = new List<TeamScore?>(),
                Players = new List<PlayerScore?>()
            });

        _teamInfoService.Verify(s => s.SetModeIsTeams(false), Times.Once);
        _teamInfoService.Verify(s => s.SetModeIsTeams(true), Times.Never);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Sets_Team_Mode_Enabled_If_Required_And_Updates_Points()
    {
        var team1Points = 4;
        var team2Points = 7;
        
        _teamInfoService.Setup(s => s.GetModeIsTeams())
            .Returns(Task.FromResult(false));
        
        await Controller.OnScores(null,
            new ScoresEventArgs
            {
                Section = ModeScriptSection.Undefined,
                UseTeams = true,
                WinnerTeam = 0,
                WinnerPlayer = null,
                Teams = new List<TeamScore?>
                {
                    new TeamScore { MatchPoints = team1Points },
                    new TeamScore { MatchPoints = team2Points },
                },
                Players = new List<PlayerScore?>()
            });

        _teamInfoService.Verify(s => s.SetModeIsTeams(true), Times.Once);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(team1Points, team2Points), Times.Once);
    }

    [Fact]
    public async Task Updates_Points_On_New_Scores_While_Already_In_Teams_Mode()
    {
        var team1Points = 45;
        var team2Points = 56;
        
        _teamInfoService.Setup(s => s.GetModeIsTeams())
            .Returns(Task.FromResult(true));
        
        await Controller.OnScores(null,
            new ScoresEventArgs
            {
                Section = ModeScriptSection.EndRound,
                UseTeams = true,
                WinnerTeam = 0,
                WinnerPlayer = null,
                Teams = new List<TeamScore?>
                {
                    new TeamScore { MatchPoints = team1Points },
                    new TeamScore { MatchPoints = team2Points },
                },
                Players = new List<PlayerScore?>()
            });

        _teamInfoService.Verify(s => s.SetModeIsTeams(false), Times.Never);
        _teamInfoService.Verify(s => s.SetModeIsTeams(true), Times.Never);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(team1Points, team2Points), Times.Once);
    }

    [Fact]
    public async Task Updated_Round_Number_On_New_Round()
    {
        var roundNumber = 777;
        await Controller.OnRoundStart(null, new RoundEventArgs { Count = roundNumber, Time = 0 });
        _teamInfoService.Verify(s => s.UpdateRoundNumberAsync(roundNumber), Times.Once);
    }

    [Fact]
    public async Task Hides_Widget_On_Podium_Start()
    {
        await Controller.OnPodiumStart(null, new PodiumEventArgs { Time = 0 });
        _teamInfoService.Verify(s => s.HideTeamInfoWidgetEveryoneAsync(), Times.Once);
    }

    [Fact]
    public async Task Hides_Widget_On_End_Map()
    {
        await Controller.OnEndMap(null, new MapGbxEventArgs());
        _teamInfoService.Verify(s => s.HideTeamInfoWidgetEveryoneAsync(), Times.Once);
    }

    [Fact]
    public async Task Sends_Widget_To_Connecting_Player()
    {
        var playerLogin = "unittest";
        await Controller.OnPlayerConnect(null, new PlayerConnectGbxEventArgs { Login = playerLogin });
        _teamInfoService.Verify(s => s.SendTeamInfoWidgetAsync(playerLogin), Times.Once);
    }
}
