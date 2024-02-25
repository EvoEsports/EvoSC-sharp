using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Services;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace EvoSC.Modules.Official.CurrentMapModule.Tests;

public class CurrentMapServiceTest
{
    private readonly IServerClient _clientMock = Substitute.For<IServerClient>();
    private readonly CurrentMapService _currentMapService;
    private readonly ILogger<CurrentMapService> _logger = new NullLogger<CurrentMapService>();
    private readonly IManialinkManager _manialinkManager = Substitute.For<IManialinkManager>();
    private readonly IMapRepository _mapRepositoryMock = Substitute.For<IMapRepository>();
    private readonly IWorldRecordService _worldRecordServiceMock = Substitute.For<IWorldRecordService>();

    public CurrentMapServiceTest()
    {
        _currentMapService =
            new CurrentMapService(_manialinkManager, _logger, _mapRepositoryMock, _clientMock,
                _worldRecordServiceMock);
    }

    [Fact]
    async Task Should_Hide_Widget()
    {
        await _currentMapService.HideWidgetAsync();
        await _manialinkManager.Received().HideManialinkAsync("CurrentMapModule.CurrentMapWidget");
    }
}
