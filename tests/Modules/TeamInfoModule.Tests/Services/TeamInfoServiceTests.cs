using EvoSC.Common.Interfaces;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Config;
using EvoSC.Modules.Official.TeamInfoModule.Interfaces;
using EvoSC.Modules.Official.TeamInfoModule.Models;
using EvoSC.Modules.Official.TeamInfoModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.Structs;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamInfoModule.Tests.Services;

public class TeamInfoServiceTests
{
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<ITeamInfoSettings> _settings = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    private ITeamInfoService TeamInfoServiceMock()
    {
        return new TeamInfoService(_server.Client.Object, _manialinkManager.Object, _settings.Object);
    }

    [Fact]
    public async Task Initializes_Module_Service()
    {
        await TeamInfoServiceMock().InitializeModuleAsync();

        _server.Remote.Verify(remote => remote.TriggerModeScriptEventArrayAsync("Trackmania.GetScores"), Times.Once);
    }

    [Theory]
    [InlineData(7, 6, 0, 1, true)]
    [InlineData(7, 6, 5, 1, true)]
    [InlineData(7, 6, 6, 1, true)]
    [InlineData(7, 0, 0, 1, false)]
    [InlineData(7, 5, 0, 1, false)]
    [InlineData(7, 6, 5, 2, true)]
    [InlineData(7, 7, 6, 2, true)]
    [InlineData(7, 6, 6, 2, false)]
    [InlineData(7, 7, 7, 2, false)]
    public async Task Detects_Map_Point(int pointsLimit, int teamPoints, int opponentPoints, int pointsGap,
        bool shouldHaveMapPoint)
    {
        Assert.Equal(
            shouldHaveMapPoint,
            await TeamInfoServiceMock().DoesTeamHaveMatchPointAsync(teamPoints, opponentPoints, pointsLimit, pointsGap)
        );
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Sets_And_Gets_Widget_Visibility(bool shouldBeVisible)
    {
        var teamInfoService = TeamInfoServiceMock();
        await teamInfoService.SetWidgetVisibilityAsync(shouldBeVisible);

        Assert.Equal(shouldBeVisible, await teamInfoService.GetWidgetVisibilityAsync());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Sets_And_Gets_Mode_Is_Teams(bool isTeamsMode)
    {
        var teamInfoService = TeamInfoServiceMock();
        await teamInfoService.SetModeIsTeamsAsync(isTeamsMode);

        Assert.Equal(isTeamsMode, await teamInfoService.GetModeIsTeamsAsync());
    }

    [Fact]
    public async Task Parses_Mode_Script_Team_Settings()
    {
        var teamInfoService = TeamInfoServiceMock();
        var mockModeSettings = new GbxDynamicObject
        {
            ["S_PointsLimit"] = 1,
            ["S_FinishTimeout"] = 2,
            ["S_MaxPointsPerRound"] = 3,
            ["S_PointsGap"] = 4,
            ["S_UseCustomPointsRepartition"] = false,
            ["S_CumulatePoints"] = false,
            ["S_RoundsPerMap"] = 7,
            ["S_MapsPerMatch"] = 8,
            ["S_UseTieBreak"] = true,
            ["S_WarmUpNb"] = 10,
            ["S_WarmUpDuration"] = 11,
            ["S_UseAlternateRules"] = false,
            ["S_NeutralEmblemUrl"] = "https://domain.tld/image.png",
        };

        _server.Remote.Setup(s => s.GetModeScriptSettingsAsync())
            .Returns(Task.FromResult(mockModeSettings));

        var modeScriptTeamSettings = await teamInfoService.GetModeScriptTeamSettingsAsync();

        _server.Remote.Verify(remote => remote.GetModeScriptSettingsAsync(), Times.Once);

        Assert.Equal(1, modeScriptTeamSettings.PointsLimit);
        Assert.Equal(2, modeScriptTeamSettings.FinishTimeout);
        Assert.Equal(3, modeScriptTeamSettings.MaxPointsPerRound);
        Assert.Equal(4, modeScriptTeamSettings.PointsGap);
        Assert.False(modeScriptTeamSettings.UseCustomPointsRepartition);
        Assert.False(modeScriptTeamSettings.CumulatePoints);
        Assert.Equal(7, modeScriptTeamSettings.RoundsPerMap);
        Assert.Equal(8, modeScriptTeamSettings.MapsPerMatch);
        Assert.True(modeScriptTeamSettings.UseTieBreak);
        Assert.Equal(10, modeScriptTeamSettings.WarmUpNb);
        Assert.Equal(11, modeScriptTeamSettings.WarmUpDuration);
        Assert.False(modeScriptTeamSettings.UseAlternateRules);
        Assert.Equal("https://domain.tld/image.png", modeScriptTeamSettings.NeutralEmblemUrl);
    }

    [Fact]
    public async Task Returns_Default_Mode_Script_Team_Settings_For_Unset_Settings()
    {
        var teamInfoService = TeamInfoServiceMock();
        var defaultModeSettings = new ModeScriptTeamSettings();
        var mockModeSettings = new GbxDynamicObject();

        _server.Remote.Setup(s => s.GetModeScriptSettingsAsync())
            .Returns(Task.FromResult(mockModeSettings));

        var modeScriptTeamSettings = await teamInfoService.GetModeScriptTeamSettingsAsync();

        _server.Remote.Verify(remote => remote.GetModeScriptSettingsAsync(), Times.Once);

        Assert.Equal(defaultModeSettings.PointsLimit, modeScriptTeamSettings.PointsLimit);
        Assert.Equal(defaultModeSettings.FinishTimeout, modeScriptTeamSettings.FinishTimeout);
        Assert.Equal(defaultModeSettings.MaxPointsPerRound, modeScriptTeamSettings.MaxPointsPerRound);
        Assert.Equal(defaultModeSettings.PointsGap, modeScriptTeamSettings.PointsGap);
        Assert.Equal(defaultModeSettings.UseCustomPointsRepartition, modeScriptTeamSettings.UseCustomPointsRepartition);
        Assert.Equal(defaultModeSettings.CumulatePoints, modeScriptTeamSettings.CumulatePoints);
        Assert.Equal(defaultModeSettings.RoundsPerMap, modeScriptTeamSettings.RoundsPerMap);
        Assert.Equal(defaultModeSettings.MapsPerMatch, modeScriptTeamSettings.MapsPerMatch);
        Assert.Equal(defaultModeSettings.UseTieBreak, modeScriptTeamSettings.UseTieBreak);
        Assert.Equal(defaultModeSettings.WarmUpNb, modeScriptTeamSettings.WarmUpNb);
        Assert.Equal(defaultModeSettings.WarmUpDuration, modeScriptTeamSettings.WarmUpDuration);
        Assert.Equal(defaultModeSettings.UseAlternateRules, modeScriptTeamSettings.UseAlternateRules);
        Assert.Equal(defaultModeSettings.NeutralEmblemUrl, modeScriptTeamSettings.NeutralEmblemUrl);
    }

    [Fact]
    public async Task Sends_Widget_To_Player()
    {
        var playerLogin = "unittest";
        var teamInfoService = TeamInfoServiceMock();
        var mockModeSettings = new GbxDynamicObject();

        _server.Remote.Setup(s => s.GetModeScriptSettingsAsync())
            .Returns(Task.FromResult(mockModeSettings));

        await teamInfoService.SendTeamInfoWidgetAsync(playerLogin);

        _server.Remote.Verify(remote => remote.GetModeScriptSettingsAsync(), Times.Once);
        _manialinkManager.Verify(
            m => m.SendManialinkAsync(playerLogin, "TeamInfoModule.TeamInfoWidget", It.IsAny<It.IsAnyType>()),
            Times.Once);
    }

    [Fact]
    public async Task Sends_Widget_To_Everyone()
    {
        var teamInfoService = TeamInfoServiceMock();

        _server.Remote.Setup(s => s.GetModeScriptSettingsAsync())
            .Returns(Task.FromResult(new GbxDynamicObject()));

        await teamInfoService.SetWidgetVisibilityAsync(false);
        await teamInfoService.SendTeamInfoWidgetEveryoneAsync();

        _server.Remote.Verify(remote => remote.GetModeScriptSettingsAsync(), Times.Once);
        _manialinkManager.Verify(
            m => m.SendManialinkAsync("TeamInfoModule.TeamInfoWidget", It.IsAny<It.IsAnyType>()),
            Times.Once);

        Assert.True(await teamInfoService.GetWidgetVisibilityAsync());
    }

    [Fact]
    public async Task Hides_Widget_For_Everyone()
    {
        var teamInfoService = TeamInfoServiceMock();

        await teamInfoService.SetWidgetVisibilityAsync(true);
        await teamInfoService.HideTeamInfoWidgetEveryoneAsync();

        _manialinkManager.Verify(
            m => m.HideManialinkAsync("TeamInfoModule.TeamInfoWidget"),
            Times.Once);

        Assert.False(await teamInfoService.GetWidgetVisibilityAsync());
    }

    [Fact]
    public async Task Updates_Round_Number()
    {
        var teamInfoService = TeamInfoServiceMock();

        _server.Remote.Setup(s => s.GetModeScriptSettingsAsync())
            .Returns(Task.FromResult(new GbxDynamicObject()));

        await teamInfoService.SetWidgetVisibilityAsync(false);
        await teamInfoService.UpdateRoundNumberAsync(777);

        _manialinkManager.Verify(
            m => m.SendManialinkAsync("TeamInfoModule.TeamInfoWidget", It.IsAny<It.IsAnyType>()),
            Times.Once);

        Assert.True(await teamInfoService.GetWidgetVisibilityAsync());
    }

    [Fact]
    public async Task Updates_Teams_Points()
    {
        var teamInfoService = TeamInfoServiceMock();

        _server.Remote.Setup(s => s.GetModeScriptSettingsAsync())
            .Returns(Task.FromResult(new GbxDynamicObject()));

        await teamInfoService.SetWidgetVisibilityAsync(false);
        await teamInfoService.UpdatePointsAsync(4, 5);

        _manialinkManager.Verify(
            m => m.SendManialinkAsync("TeamInfoModule.TeamInfoWidget", It.IsAny<It.IsAnyType>()),
            Times.Once);

        Assert.True(await teamInfoService.GetWidgetVisibilityAsync());
    }

    [Theory]
    [InlineData(5, 1, 0, "FIRST TO 5")]
    [InlineData(6, 2, 0, "FIRST TO 6 | GAP 2")]
    [InlineData(7, 3, 0, "FIRST TO 7 | GAP 3")]
    [InlineData(0, 1, 3, "3 ROUNDS/MAP")]
    [InlineData(9, 1, 4, "FIRST TO 9 | 4 ROUNDS/MAP")]
    [InlineData(9, 5, 4, "FIRST TO 9 | GAP 5 | 4 ROUNDS/MAP")]
    [InlineData(0, 1, 0, null)]
    public async Task Generates_Info_Box_Text(int pointsLimit, int pointsGap, int roundsPerMap, string? expected)
    {
        var teamInfoService = TeamInfoServiceMock();
        var modeScriptTeamSettings = new ModeScriptTeamSettings
        {
            PointsLimit = pointsLimit, PointsGap = pointsGap, RoundsPerMap = roundsPerMap
        };

        Assert.Equal(expected, await teamInfoService.GetInfoBoxTextAsync(modeScriptTeamSettings));
    }

    [Fact]
    public async Task Gets_Widget_Data()
    {
        var teamInfoService = TeamInfoServiceMock();
        var defaultTeamSettings = new ModeScriptTeamSettings();
        var team1Info = new TmTeamInfo { Name = "unit1" };
        var team2Info = new TmTeamInfo { Name = "unit2" };
        var expectedTeam1Points = defaultTeamSettings.PointsLimit - 1;
        var expectedTeam2Points = 0;
        var expectedRoundNumber = 777;

        _server.Remote.Setup(s => s.GetModeScriptSettingsAsync())
            .Returns(Task.FromResult(new GbxDynamicObject()));
        _server.Remote.Setup(s => s.GetTeamInfoAsync(1))
            .Returns(Task.FromResult(team1Info));
        _server.Remote.Setup(s => s.GetTeamInfoAsync(2))
            .Returns(Task.FromResult(team2Info));

        await teamInfoService.UpdateRoundNumberAsync(expectedRoundNumber);
        await teamInfoService.UpdatePointsAsync(expectedTeam1Points, expectedTeam2Points);
        var widgetData = await teamInfoService.GetWidgetDataAsync();

        _server.Remote.Verify(remote => remote.GetModeScriptSettingsAsync(), Times.Exactly(3));
        _server.Remote.Verify(remote => remote.GetTeamInfoAsync(1), Times.Exactly(3));
        _server.Remote.Verify(remote => remote.GetTeamInfoAsync(2), Times.Exactly(3));

        var team1Property = widgetData.GetType().GetProperty("team1");
        var returnedTeam1Data = team1Property.GetValue(widgetData, null);
        Assert.IsType<TmTeamInfo>(returnedTeam1Data);
        Assert.Equal("unit1", returnedTeam1Data.Name);

        var team2Property = widgetData.GetType().GetProperty("team2");
        var returnedTeam2Data = team2Property.GetValue(widgetData, null);
        Assert.IsType<TmTeamInfo>(returnedTeam2Data);
        Assert.Equal("unit2", returnedTeam2Data.Name);

        var infoBoxText = widgetData.GetType().GetProperty("infoBoxText");
        var returnedInfoBoxText = infoBoxText.GetValue(widgetData, null);
        Assert.Equal(await teamInfoService.GetInfoBoxTextAsync(defaultTeamSettings), returnedInfoBoxText);

        var team1MatchPointProperty = widgetData.GetType().GetProperty("team1MatchPoint");
        var returnedTeam1MatchPoint = team1MatchPointProperty.GetValue(widgetData, null);
        Assert.IsType<bool>(returnedTeam1MatchPoint);
        Assert.True(returnedTeam1MatchPoint);

        var team2MatchPointProperty = widgetData.GetType().GetProperty("team2MatchPoint");
        var returnedTeam2MatchPoint = team2MatchPointProperty.GetValue(widgetData, null);
        Assert.IsType<bool>(returnedTeam2MatchPoint);
        Assert.False(returnedTeam2MatchPoint);

        var roundNumberProperty = widgetData.GetType().GetProperty("roundNumber");
        var returnedRoundNumber = roundNumberProperty.GetValue(widgetData, null);
        Assert.Equal(expectedRoundNumber, returnedRoundNumber);

        var team1PointsProperty = widgetData.GetType().GetProperty("team1Points");
        var returnedTeam1Points = team1PointsProperty.GetValue(widgetData, null);
        Assert.Equal(expectedTeam1Points, returnedTeam1Points);

        var team2PointsProperty = widgetData.GetType().GetProperty("team2Points");
        var returnedTeam2Points = team2PointsProperty.GetValue(widgetData, null);
        Assert.Equal(expectedTeam2Points, returnedTeam2Points);

        var neutralEmblemUrlProperty = widgetData.GetType().GetProperty("neutralEmblemUrl");
        var returnedNeutralEmblemUrl = neutralEmblemUrlProperty.GetValue(widgetData, null);
        Assert.Empty(returnedNeutralEmblemUrl);
    }
}
