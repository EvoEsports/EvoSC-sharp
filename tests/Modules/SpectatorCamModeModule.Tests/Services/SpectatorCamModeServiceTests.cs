using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Enums;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Models;
using EvoSC.Modules.Official.SpectatorCamModeModule.Config;
using EvoSC.Modules.Official.SpectatorCamModeModule.Interfaces;
using EvoSC.Modules.Official.SpectatorCamModeModule.Services;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.SpectatorCamModeModule.Tests.Services;

public class SpectatorCamModeServiceTests
{
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<IGameModeUiModuleService> _gameModeUiModule = new();
    private readonly Mock<ISpectatorCamModeSettings> _settings = new();

    private ISpectatorCamModeService CamModeServiceMock()
    {
        return new SpectatorCamModeService(
            _manialinkManager.Object,
            _gameModeUiModule.Object,
            _settings.Object
        );
    }

    [Fact]
    public async Task SendsPersistentCamModeWidget()
    {
        var camModeService = CamModeServiceMock();
        await camModeService.SendPersistentCamModeWidgetAsync();

        _manialinkManager.Verify(m =>
            m.SendPersistentManialinkAsync("SpectatorCamModeModule.SpectatorMode", It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task HidesDefaultGameModeUi()
    {
        var camModeService = CamModeServiceMock();
        await camModeService.HideGameModeUiAsync();

        _gameModeUiModule.Verify(s => s.ApplyComponentSettingsAsync(It.IsAny<GameModeUiComponentSettings>()));
    }

    [Fact]
    public Task GameModeUiSettingsHideCorrectComponent()
    {
        var camModeService = CamModeServiceMock();
        var gameModeUiSettings = camModeService.GetGameModeUiSettings();
        
        Assert.Equal(GameModeUiComponents.SpectatorBaseCommands, gameModeUiSettings.Name);
        Assert.False(gameModeUiSettings.Visible);

        return Task.CompletedTask;
    }
}
