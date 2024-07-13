using System.Dynamic;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Controllers;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamSettingsModule.Tests.Controllers;

public class TeamSettingsManialinkControllerTests : ManialinkControllerTestBase<TeamSettingsManialinkController>
{
    private Mock<IOnlinePlayer> _player = new();
    private Mock<IManialinkActionContext> _manialinkActionContext = new();
    private Mock<ITeamSettingsService> _teamSettingsService = new();

    public TeamSettingsManialinkControllerTests()
    {
        InitMock(_player.Object, _manialinkActionContext.Object, _teamSettingsService,
            Mocking.NewLocaleMock(ContextService.Object));
    }

    [Fact]
    public async Task Team_Settings_Updated()
    {
        var teamSettings = new TeamSettingsModel();

        await Controller.ValidateModelAsync();
        await Controller.SaveTeamSettingsAsync(teamSettings);

        _teamSettingsService.Verify(m => m.SetTeamSettingsAsync(teamSettings), Times.Once);
        _teamSettingsService.Verify(m => m.HideTeamSettingsAsync(Context.Object.Player));
        AuditEventBuilder.Verify(m => m.Success());
    }

    [Theory]
    [InlineData("", "fff", null, null, "Test2", "fff", null, null)]
    [InlineData("Test1", "", null, null, "Test2", "fff", null, null)]
    [InlineData("Test1", "fff", "invalid", null, "Test2", "fff", null, null)]
    [InlineData("Test1", "fff", null, "invalid", "Test2", "fff", null, null)]
    [InlineData("Test1", "fff", null, null, "", "fff", null, null)]
    [InlineData("Test1", "fff", null, null, "Test2", "", null, null)]
    [InlineData("Test1", "fff", null, null, "Test2", "fff", "invalid", null)]
    [InlineData("Test1", "fff", null, null, "Test2", "fff", null, "invalid")]
    public async Task Team_Settings_Do_Not_Update_With_Invalid_Data(
        string team1Name,
        string team1PrimaryColor,
        string? team1SecondaryColor,
        string? team1EmblemUrl,
        string team2Name,
        string team2PrimaryColor,
        string? team2SecondaryColor,
        string? team2EmblemUrl
    )
    {
        var teamSettings = new TeamSettingsModel
        {
            Team1Name = team1Name,
            Team1PrimaryColor = team1PrimaryColor,
            Team1SecondaryColor = team1SecondaryColor,
            Team1EmblemUrl = team1EmblemUrl,
            Team2Name = team2Name,
            Team2PrimaryColor = team2PrimaryColor,
            Team2SecondaryColor = team2SecondaryColor,
            Team2EmblemUrl = team2EmblemUrl
        };

        _manialinkActionContext.Setup(m => m.EntryModel).Returns(teamSettings);

        var validationResult = await Controller.ValidateModelAsync();
        Assert.False(validationResult.IsValid);
        await Controller.SaveTeamSettingsAsync(teamSettings);
        
        _teamSettingsService.Verify(m => m.SetTeamSettingsAsync(teamSettings), Times.Never);
        _teamSettingsService.Verify(m => m.HideTeamSettingsAsync(_player.Object), Times.Never);
        ManialinkManager.Verify(m => m.SendManialinkAsync(_player.Object, "TeamSettingsModule.EditTeamSettings", It.IsAny<ExpandoObject>()), Times.Once);
    }
}
