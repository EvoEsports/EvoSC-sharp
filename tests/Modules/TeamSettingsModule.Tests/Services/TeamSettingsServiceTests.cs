using System.Collections.Specialized;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Events;
using EvoSC.Modules.Official.TeamSettingsModule.Events.EventArgs;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.Structs;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamSettingsModule.Tests.Services;

public class TeamSettingsServiceTests
{
    private readonly Mock<IOnlinePlayer> _player = new();
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<IEventManager> _events = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat)
        _server = Mocking.NewServerClientMock();

    private ITeamSettingsService TeamSettingsServiceMock()
    {
        var mlAction = new Mock<IManialinkActionContext>();
        var server = Mocking.NewServerClientMock();
        var context =
            Mocking.NewManialinkInteractionContextMock(server.Client, _player.Object, mlAction.Object, _manialinkManager.Object);
        var contextService = Mocking.NewContextServiceMock(context.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);

        return new TeamSettingsService(_server.Client.Object, _manialinkManager.Object, _events.Object, locale);
    }

    [Theory]
    [InlineData("TestTeam1", "000", null, null)]
    [InlineData("TestTeam2", "f0f", "321", null)]
    [InlineData("TestTeam3", "0f0", "123", "https://some.domain/image.png")]
    [InlineData("TestTeam4", "0f0", null, "https://some.domain/image.png")]
    [InlineData("TestTeam5", "000000", null, null)]
    [InlineData("TestTeam6", "000000", "ffffff", null)]
    public async Task Generates_And_Parses_Club_Link_URL(string teamName, string primaryColor, string? secondaryColor,
        string? emblemUrl)
    {
        var teamSettingsService = TeamSettingsServiceMock();
        var clubLinkUrl = await teamSettingsService.GenerateClubLinkUrl(
            teamName,
            primaryColor,
            secondaryColor,
            emblemUrl
        );

        Assert.True(Uri.IsWellFormedUriString(clubLinkUrl, UriKind.Absolute));

        var parsedTeamSettings = await teamSettingsService.ParseClubLinkUrl(clubLinkUrl);

        Assert.Equal(teamName, parsedTeamSettings.Get("name"));
        Assert.Equal(primaryColor, parsedTeamSettings.Get("primary"));
        Assert.Equal(secondaryColor, parsedTeamSettings.Get("secondary"));
        Assert.Equal(emblemUrl, parsedTeamSettings.Get("emblem"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Returns_Default_Team_Settings_If_ClubLink_Is_Empty(string? clubLinkUrl)
    {
        var teamSettingsService = TeamSettingsServiceMock();

        Assert.Equal([], await teamSettingsService.ParseClubLinkUrl(clubLinkUrl));
    }

    [Fact]
    public async Task Shows_Team_Settings()
    {
        var teamSettings = new TeamSettingsModel();

        await TeamSettingsServiceMock().ShowTeamSettingsAsync(_player.Object, teamSettings);

        _manialinkManager.Verify(
            m => m.SendManialinkAsync(_player.Object, "TeamSettingsModule.EditTeamSettings", It.IsAny<It.IsAnyType>()),
            Times.Once);
    }

    [Fact]
    public async Task Hides_Team_Settings_Editor()
    {
        await TeamSettingsServiceMock().HideTeamSettingsAsync(_player.Object);

        _manialinkManager.Verify(m => m.HideManialinkAsync(_player.Object, "TeamSettingsModule.EditTeamSettings"),
            Times.Once);
    }

    [Fact]
    public async Task Sets_Team_Settings()
    {
        var teamSettingsService = TeamSettingsServiceMock();
        var teamSettings = new TeamSettingsModel { Team1Name = "Test1", Team2Name = "Test2", };

        await teamSettingsService.SetTeamSettingsAsync(teamSettings);

        var clubLinkUrlTeam1 = await teamSettingsService.GenerateClubLinkUrl(
            "Test1",
            teamSettings.Team1PrimaryColor,
            teamSettings.Team1SecondaryColor,
            teamSettings.Team1EmblemUrl
        );

        var clubLinkUrlTeam2 = await teamSettingsService.GenerateClubLinkUrl(
            "Test2",
            teamSettings.Team2PrimaryColor,
            teamSettings.Team2SecondaryColor,
            teamSettings.Team2EmblemUrl
        );

        _server.Remote.Verify(m => m.SetForcedClubLinksAsync(clubLinkUrlTeam1, clubLinkUrlTeam2), Times.Once);
        _events.Verify(m => m.RaiseAsync(TeamSettingsEvents.SettingsUpdated, It.IsAny<TeamSettingsEventArgs>()));
    }

    [Fact]
    public async Task Gets_Current_Team_Settings()
    {
        var teamSettingsService = TeamSettingsServiceMock();
        var team1Info = new TmTeamInfo { Name = "Blue" };
        var team2Info = new TmTeamInfo { Name = "Red" };
        var expectedTeamSettings = new TeamSettingsModel();

        _server.Remote.Setup(m => m.GetTeamInfoAsync(1)).Returns(Task.FromResult(team1Info));
        _server.Remote.Setup(m => m.GetTeamInfoAsync(2)).Returns(Task.FromResult(team2Info));

        var retrievedTeamSettings = await teamSettingsService.GetCurrentTeamSettingsModel();

        Assert.Equal(expectedTeamSettings.Team1Name, retrievedTeamSettings.Team1Name);
        Assert.Equal(expectedTeamSettings.Team1PrimaryColor, retrievedTeamSettings.Team1PrimaryColor);
        Assert.Equal(expectedTeamSettings.Team1SecondaryColor, retrievedTeamSettings.Team1SecondaryColor);
        Assert.Equal(expectedTeamSettings.Team1EmblemUrl, retrievedTeamSettings.Team1EmblemUrl);
        Assert.Equal(expectedTeamSettings.Team2Name, retrievedTeamSettings.Team2Name);
        Assert.Equal(expectedTeamSettings.Team2PrimaryColor, retrievedTeamSettings.Team2PrimaryColor);
        Assert.Equal(expectedTeamSettings.Team2SecondaryColor, retrievedTeamSettings.Team2SecondaryColor);
        Assert.Equal(expectedTeamSettings.Team2EmblemUrl, retrievedTeamSettings.Team2EmblemUrl);
    }
}
