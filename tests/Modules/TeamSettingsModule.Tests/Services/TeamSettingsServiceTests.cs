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

    [Fact]
    public async Task Generates_Club_Link_URL()
    {
        var teamSettingsService = TeamSettingsServiceMock();
        var clubLinkUrl = await teamSettingsService.GenerateClubLinkUrl("testteam", "ff0066");

        Assert.True(Uri.IsWellFormedUriString(clubLinkUrl, UriKind.Absolute));
    }

    [Fact]
    public async Task Team_Settings_Xml_Is_Generated()
    {
        //TODO: check that the returned XML is valid
    }

    [Fact]
    public async Task Team_Settings_Is_Updated()
    {
        //TODO: check that event fired, after updating team settings
    }
}
