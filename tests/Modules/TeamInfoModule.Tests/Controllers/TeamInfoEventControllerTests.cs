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
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));
        
        await Controller.OnScoresAsync(null,
            new ScoresEventArgs
            {
                Section = ModeScriptSection.Undefined,
                UseTeams = false,
                WinnerTeam = 0,
                WinnerPlayer = null,
                Teams = new List<TeamScore?>(),
                Players = new List<PlayerScore?>()
            });

        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(false), Times.Once);
        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(true), Times.Never);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Sets_Team_Mode_Enabled_If_Required_And_Updates_Points()
    {
        var team1Points = 4;
        var team2Points = 7;
        
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(false));
        
        await Controller.OnScoresAsync(null,
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

        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(true), Times.Once);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(team1Points, team2Points), Times.Once);
    }

    [Fact]
    public async Task Updates_Points_On_New_Scores_While_Already_In_Teams_Mode()
    {
        var team1Points = 45;
        var team2Points = 56;
        
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));
        
        await Controller.OnScoresAsync(null,
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

        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(false), Times.Never);
        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(true), Times.Never);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(team1Points, team2Points), Times.Once);
    }

    [Fact]
    public async Task Updates_Round_Number_On_New_Round()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));
        
        var roundNumber = 777;
        await Controller.OnRoundStartAsync(null, new RoundEventArgs { Count = roundNumber, Time = 0 });
        _teamInfoService.Verify(s => s.UpdateRoundNumberAsync(roundNumber), Times.Once);
    }

    [Fact]
    public async Task Doesnt_Update_Round_Number_On_New_Round_If_Mode_Is_Not_Teams()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(false));
        
        await Controller.OnRoundStartAsync(null, new RoundEventArgs { Count = 0, Time = 0 });
        _teamInfoService.Verify(s => s.UpdateRoundNumberAsync(0), Times.Never);
    }

    [Fact]
    public async Task Hides_Widget_On_Podium_Start()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));
        
        await Controller.OnPodiumStartAsync(null, new PodiumEventArgs { Time = 0 });
        _teamInfoService.Verify(s => s.HideTeamInfoWidgetEveryoneAsync(), Times.Once);
    }

    [Fact]
    public async Task Hides_Widget_On_End_Map()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));
        
        await Controller.OnEndMap(null, new MapGbxEventArgs());
        _teamInfoService.Verify(s => s.HideTeamInfoWidgetEveryoneAsync(), Times.Once);
    }

    [Fact]
    public async Task Sends_Widget_To_Connecting_Player()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));
        _teamInfoService.Setup(s => s.GetWidgetVisibilityAsync())
            .Returns(Task.FromResult(true));
        
        var playerLogin = "unittest";
        await Controller.OnPlayerConnectAsync(null, new PlayerConnectGbxEventArgs { Login = playerLogin });
        _teamInfoService.Verify(s => s.SendTeamInfoWidgetAsync(playerLogin), Times.Once);
    }

    [Fact]
    public async Task Does_Not_Send_Widget_To_Connecting_Player_If_Mode_Is_Not_Teams()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(false));
        _teamInfoService.Setup(s => s.GetWidgetVisibilityAsync())
            .Returns(Task.FromResult(false));
        
        await Controller.OnPlayerConnectAsync(null, new PlayerConnectGbxEventArgs());
        _teamInfoService.Verify(s => s.SendTeamInfoWidgetAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Does_Not_Send_Widget_To_Connecting_Player_When_Widget_Is_Hidden()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));
        _teamInfoService.Setup(s => s.GetWidgetVisibilityAsync())
            .Returns(Task.FromResult(false));
        
        await Controller.OnPlayerConnectAsync(null, new PlayerConnectGbxEventArgs());
        _teamInfoService.Verify(s => s.SendTeamInfoWidgetAsync(It.IsAny<string>()), Times.Never);
    }
}
