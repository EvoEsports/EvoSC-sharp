using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Models.Players;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Services;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Official.CurrentMapModule.Tests;

public class CurrentMapServiceTest
{
    private readonly Mock<IServerClient> _clientMock = new();
    private readonly CurrentMapService _currentMapService;
    private readonly ILogger<CurrentMapService> _logger = new NullLogger<CurrentMapService>();
    private readonly Mock<IManialinkManager> _manialinkManager = new();

    private readonly IMap _map = new Map { Author = new Player { Zone = "||Germany" } };
    private readonly Mock<IMapRepository> _mapRepositoryMock = new();
    private readonly Mock<IEvoScBaseConfig> _configMock = new();
    private readonly Mock<IWorldRecordService> _worldRecordServiceMock = new();
    private readonly Mock<IThemeManager> _themeManagerMock = new();

    public CurrentMapServiceTest()
    {
        _currentMapService =
            new CurrentMapService(_manialinkManager.Object, _logger, _mapRepositoryMock.Object, _clientMock.Object,
                _worldRecordServiceMock.Object);
    }

    [Fact]
    async void Should_Hide_Widget()
    {
        await _currentMapService.HideWidgetAsync();
        _manialinkManager.Verify(manager => manager.HideManialinkAsync("CurrentMapModule.CurrentMapWidget"));
    }
}
