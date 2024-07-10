using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamSettingsModule.Tests.Services;

public class TeamSettingsServiceTests
{
    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    private ITeamSettingsService TeamSettingsServiceMock()
    {
        var player = new Mock<IOnlinePlayer>();
        var mlAction = new Mock<IManialinkActionContext>();
        var mlManager = new Mock<IManialinkManager>();
        var context = Mocking.NewManialinkInteractionContextMock(player.Object, mlAction.Object, mlManager.Object);
        var contextService = Mocking.NewContextServiceMock(context.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);
        var manialinkManagerService = new Mock<IManialinkManager>();
        var teamSettingsService = new TeamSettingsService(_server.Client.Object, manialinkManagerService.Object, locale);

        return teamSettingsService;
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

    [Fact]
    public async Task Sets_Team_Settings()
    {
        var teamSettingsService = TeamSettingsServiceMock();
        var teamSettings = new TeamSettingsModel
        {
            Team1Name = "Test1",
            Team2Name = "Test2",
        };

        await teamSettingsService.SetTeamSettingsAsync(teamSettings);

        var clubLinkUrlTeam1 = await teamSettingsService.GenerateClubLinkUrl(teamSettings.Team1Name, teamSettings.Team1PrimaryColor, teamSettings.Team1SecondaryColor, teamSettings.Team1EmblemUrl);
        var clubLinkUrlTeam2 = await teamSettingsService.GenerateClubLinkUrl(teamSettings.Team2Name, teamSettings.Team2PrimaryColor, teamSettings.Team2SecondaryColor, teamSettings.Team2EmblemUrl);

        _server.Remote.Verify(m => m.SetForcedClubLinksAsync(clubLinkUrlTeam1, clubLinkUrlTeam2), Times.Once);
    }
}
