using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Config;
using EvoSC.Modules.Official.CurrentMapModule.Services;
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
    private readonly Mock<ICurrentMapSettings> _settings = new();
    private readonly Mock<IMapRepository> _mapRepositoryMock = new();

    public CurrentMapServiceTest()
    {
        _currentMapService =
            new CurrentMapService(_manialinkManager.Object, _logger, _mapRepositoryMock.Object, _clientMock.Object, _settings.Object);
    }

    [Fact]
    async Task Should_Hide_Widget()
    {
        await _currentMapService.HideWidgetAsync();
        _manialinkManager.Verify(manager => manager.HideManialinkAsync("CurrentMapModule.CurrentMapWidget"));
    }
}
