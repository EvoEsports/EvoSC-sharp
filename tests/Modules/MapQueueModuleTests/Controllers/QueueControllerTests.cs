using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Tests;
using EvoSC.Modules.Official.MapQueueModule.Controllers;
using EvoSC.Modules.Official.MapQueueModule.Events.Args;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.Structs;
using Moq;

namespace EvoSC.Modules.Official.MapQueueModuleTests.Tests.Controllers;

public class QueueControllerTests : EventControllerTestBase<QueueController>
{
    private Mock<IMapQueueService> _mapQueueServiceMock = new();
    private Mock<IMapService> _mapServiceMock = new();
    private (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) _server = Mocking.NewServerClientMock();
    
    public QueueControllerTests()
    {
        InitMock(_mapQueueServiceMock, _server.Client, _mapServiceMock, TestLoggerSetup.CreateLogger<QueueController>());
    }

    [Fact]
    public async Task BeginMap_Drops_First_Queued_Maps_If_Current()
    {
        var map = new Map();
        _mapServiceMock.Setup(m => m.GetCurrentMapAsync()).Returns(Task.FromResult((IMap)map));

        await Controller.OnBeginMapAsync(new object(), new MapGbxEventArgs());
        
        _mapQueueServiceMock.Verify(m => m.DropAsync(map));
    }

    [Fact]
    public async Task Begin_Map_Chooses_Next_Map_In_Queue_If_Queue_Not_Empty_And_Current_Map_Isnt_Next()
    {
        var map = new Map { Uid = "map", FilePath = "mapfile" };
        
        _mapServiceMock.Setup(m => m.GetCurrentMapAsync()).Returns(Task.FromResult((IMap)map));
        _mapQueueServiceMock.Setup(m => m.DropAsync(map)).Throws<Exception>();
        _mapQueueServiceMock.Setup(m => m.QueuedMapsCount).Returns(1);
        _mapQueueServiceMock.Setup(m => m.PeekNextAsync()).Returns(Task.FromResult((IMap)map));

        await Controller.OnBeginMapAsync(new object(), new MapGbxEventArgs { Map = new TmSMapInfo { Uid = "currentmap" } });
        
        _server.Remote.Verify(m => m.ChooseNextMapAsync(map.FilePath));
    }
    
    [Fact]
    public async Task Next_Map_Is_Chosen_On_Map_Queued_If_Its_The_Only_Queued()
    {
        var map = new Map { Uid = "map", FilePath = "mapfile" };
        _mapQueueServiceMock.Setup(m => m.QueuedMapsCount).Returns(1);

        await Controller.OnMapQueuedAsync(new object(), new MapQueueEventArgs { Map = map });
        
        _server.Remote.Verify(m => m.ChooseNextMapAsync(map.FilePath));
    }
    
    [Fact]
    public async Task Next_Map_Is_Not_Chosen_On_Map_Queued_If_Its_Not_The_Only_Queued()
    {
        var map = new Map { Uid = "map", FilePath = "mapfile" };
        _mapQueueServiceMock.Setup(m => m.QueuedMapsCount).Returns(2);

        await Controller.OnMapQueuedAsync(new object(), new MapQueueEventArgs { Map = map });
        
        _server.Remote.Verify(m => m.ChooseNextMapAsync(map.FilePath), Times.Never);
    }

    [Fact]
    public async Task Next_Queued_Map_Is_Chosen_When_Map_Is_Dropped_And_There_Are_More_Queued()
    {
        var map = new Map { Uid = "map", FilePath = "mapfile" };
        _mapQueueServiceMock.Setup(m => m.QueuedMapsCount).Returns(2);
        _mapQueueServiceMock.Setup(m => m.PeekNextAsync()).Returns(Task.FromResult((IMap)map));

        await Controller.OnMapDroppedAsync(new object(),
            new MapQueueMapDroppedEventArgs { Map = map, WasNext = false });

        _server.Remote.Verify(m => m.ChooseNextMapAsync(map.FilePath));
    }
    
    [Fact]
    public async Task No_Next_Map_Chosen_If_Map_Wasnt_Next()
    {
        var map = new Map { Uid = "map", FilePath = "mapfile" };
        var map2 = new Map { Uid = "map2", FilePath = "mapfile2" };
        _mapQueueServiceMock.Setup(m => m.QueuedMapsCount).Returns(4);
        _mapQueueServiceMock.Setup(m => m.PeekNextAsync()).Returns(Task.FromResult((IMap)map2));

        await Controller.OnMapDroppedAsync(new object(),
            new MapQueueMapDroppedEventArgs { Map = map, WasNext = true });

        _server.Remote.Verify(m => m.ChooseNextMapAsync(map2.FilePath));
        _server.Remote.Verify(m => m.ChooseNextMapAsync(map.FilePath), Times.Never);
    }
}
