using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Models.Players;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Services;
using GbxRemoteNet.Events;
using GbxRemoteNet.Structs;
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

    public CurrentMapServiceTest()
    {
        _currentMapService =
            new CurrentMapService(_manialinkManager.Object, _logger, _mapRepositoryMock.Object, _clientMock.Object);
    }

    [Fact]
    async void Should_Show_Widget()
    {
        var map = new Map { Author = new Player { Zone = "||Germany" } };
        _mapRepositoryMock.Setup(repository => repository.GetMapByUidAsync("uid")).ReturnsAsync(map);
        await _currentMapService.ShowWidgetAsync(new MapGbxEventArgs { Map = new TmSMapInfo { Uid = "uid" } });

        _mapRepositoryMock.Verify(repository => repository.GetMapByUidAsync("uid"));
        _manialinkManager.Verify(manager => manager.SendPersistentManialinkAsync("CurrentMapModule.CurrentMapWidget",
            It.Is<object>(o =>
                "DEU".Equals(o.GetType().GetProperty("country")!.GetValue(o)) &&
                map.Equals(o.GetType().GetProperty("map")!.GetValue(o)))
        ));
    }

    [Fact]
    async void Should_Show_Widget_Without_Country()
    {
        var map = new Map();
        _mapRepositoryMock.Setup(repository => repository.GetMapByUidAsync("uid")).ReturnsAsync(map);
        await _currentMapService.ShowWidgetAsync(new MapGbxEventArgs() { Map = new TmSMapInfo { Uid = "uid" } });

        _mapRepositoryMock.Verify(repository => repository.GetMapByUidAsync("uid"));
        _manialinkManager.Verify(manager => manager.SendPersistentManialinkAsync("CurrentMapModule.CurrentMapWidget",
            It.Is<object>(o =>
                "WOR".Equals(o.GetType().GetProperty("country")!.GetValue(o)) &&
                map.Equals(o.GetType().GetProperty("map")!.GetValue(o)))));
    }

    [Fact]
    async void Should_Hide_Widget()
    {
        await _currentMapService.HideWidgetAsync();
        _manialinkManager.Verify(manager => manager.HideManialinkAsync("CurrentMapModule.CurrentMapWidget"));
    }
}
