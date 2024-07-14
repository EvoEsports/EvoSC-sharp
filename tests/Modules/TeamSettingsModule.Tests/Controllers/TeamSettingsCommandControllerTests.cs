using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.TeamSettingsModule.Controllers;
using EvoSC.Modules.Official.TeamSettingsModule.Interfaces;
using EvoSC.Modules.Official.TeamSettingsModule.Models;
using EvoSC.Testing.Controllers;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.TeamSettingsModule.Tests.Controllers;

public class TeamSettingsCommandControllerTests : CommandInteractionControllerTestBase<TeamSettingsCommandsController>
{
    private Mock<ITeamSettingsService> _teamSettingsService = new();
    private Mock<IOnlinePlayer> _player = new();

    public TeamSettingsCommandControllerTests()
    {
        InitMock(_player.Object, _teamSettingsService);
    }

    [Fact]
    public async Task Team_Settings_Editor_Is_Being_Shown()
    {
        var teamSettings = new TeamSettingsModel();

        _teamSettingsService.Setup(m => m.GetCurrentTeamSettingsModel()).ReturnsAsync(teamSettings);

        await Controller.EditTeamSettingsAsync();

        _teamSettingsService.Verify(m => m.ShowTeamSettingsAsync(_player.Object, teamSettings));
    }
}
