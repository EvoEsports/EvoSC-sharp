using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Services;
using EvoSC.Testing;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamSettingsModule.Tests.Services;

public class TeamSettingsServiceTests
{
    private ITeamSettingsService TeamSettingsServiceMock()
    {
        var player = new Mock<IOnlinePlayer>();
        var mlAction = new Mock<IManialinkActionContext>();
        var mlManager = new Mock<IManialinkManager>();
        var context = Mocking.NewManialinkInteractionContextMock(player.Object, mlAction.Object, mlManager.Object);
        var contextService = Mocking.NewContextServiceMock(context.Context.Object, null);
        var locale = Mocking.NewLocaleMock(contextService.Object);
        var manialinkManagerService = new Mock<IManialinkManager>();
        var server = Mocking.NewServerClientMock();
        var teamSettingsService = new TeamSettingsService(server.Client.Object, manialinkManagerService.Object, locale);

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
}
