using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Modules.Official.TeamInfoModule.Controllers;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Services;
using EvoSC.Modules.Official.TeamSettingsModule.Events.EventArgs;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;
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
        _teamInfoService.Verify(s => s.UpdatePointsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()),
            Times.Never);
    }

    [Fact]
    public async Task Sets_Team_Mode_Enabled_If_Required_And_Updates_Points()
    {
        var team1Points = 4;
        var team2Points = 7;

        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(false));

        _teamInfoService.Setup(s => s.ShouldUpdateTeamPoints(ModeScriptSection.EndRound))
            .Returns(true);

        _teamInfoService.Setup(s => s.ShouldExecuteManiaScript(ModeScriptSection.EndRound))
            .Returns(false);

        await Controller.OnScoresAsync(null,
            new ScoresEventArgs
            {
                Section = ModeScriptSection.EndRound,
                UseTeams = true,
                WinnerTeam = 0,
                WinnerPlayer = null,
                Teams = new List<TeamScore?>
                {
                    new TeamScore { MatchPoints = team1Points }, new TeamScore { MatchPoints = team2Points },
                },
                Players = new List<PlayerScore?>()
            });

        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(true), Times.Once);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(team1Points, team2Points, false), Times.Once);
    }

    [Fact]
    public async Task Updates_Points_On_New_Scores_While_Already_In_Teams_Mode()
    {
        var team1Points = 45;
        var team2Points = 56;

        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));

        _teamInfoService.Setup(s => s.ShouldUpdateTeamPoints(ModeScriptSection.EndRound))
            .Returns(true);

        _teamInfoService.Setup(s => s.ShouldExecuteManiaScript(ModeScriptSection.EndRound))
            .Returns(false);

        await Controller.OnScoresAsync(null,
            new ScoresEventArgs
            {
                Section = ModeScriptSection.EndRound,
                UseTeams = true,
                WinnerTeam = 0,
                WinnerPlayer = null,
                Teams = new List<TeamScore?>
                {
                    new TeamScore { MatchPoints = team1Points }, new TeamScore { MatchPoints = team2Points },
                },
                Players = new List<PlayerScore?>()
            });

        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(false), Times.Never);
        _teamInfoService.Verify(s => s.SetModeIsTeamsAsync(true), Times.Never);
        _teamInfoService.Verify(s => s.UpdatePointsAsync(team1Points, team2Points, false), Times.Once);
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
    public async Task Resets_Points_On_Match_Start()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));

        await Controller.OnMatchStartAsync(null, new MatchEventArgs { Time = 0, Count = 0 });
        _teamInfoService.Verify(s => s.UpdatePointsAsync(0, 0, false), Times.Once);
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

        await Controller.OnEndMapAsync(null, new MapGbxEventArgs());
        _teamInfoService.Verify(s => s.HideTeamInfoWidgetEveryoneAsync(), Times.Once);
    }

    [Fact]
    public async Task Updates_Widget_On_New_Team_Settings()
    {
        _teamInfoService.Setup(s => s.GetModeIsTeamsAsync())
            .Returns(Task.FromResult(true));

        await Controller.OnTeamSettingsUpdatedAsync(null, new TeamSettingsEventArgs
        {
            Settings = new TeamSettingsModel()
        });
        
        _teamInfoService.Verify(s => s.SendTeamInfoWidgetEveryoneAsync(), Times.Once);
    }
}
