using EvoSC.Common.Interfaces.Localization;
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

        await Controller.SaveTeamSettingsAsync(teamSettings);

        _teamSettingsService.Verify(m => m.SetTeamSettingsAsync(teamSettings));
        _teamSettingsService.Verify(m => m.HideTeamSettingsAsync(Context.Object.Player));
        AuditEventBuilder.Verify(m => m.Success());
    }
}
