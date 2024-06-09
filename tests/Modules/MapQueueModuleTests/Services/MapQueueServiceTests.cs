using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Modules.Official.MapQueueModule.Events;
using EvoSC.Modules.Official.MapQueueModule.Events.Args;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;
using EvoSC.Modules.Official.MapQueueModule.Services;
using Moq;

namespace EvoSC.Modules.Official.MapQueueModuleTests.Tests.Services;

public class MapQueueServiceTests
{
    private (IMapQueueService MapQueueService, Mock<IEventManager> EventManager) NewMapQueueServiceMock()
    {
        var eventsService = new Mock<IEventManager>();
        var mapQueueService = new MapQueueService(eventsService.Object);

        return (mapQueueService, eventsService);
    }

    [Fact]
    public async Task Map_Added_To_Queue_Raises_Event()
    {
        var mock = NewMapQueueServiceMock();
        var map = new Map();
        
        await mock.MapQueueService.EnqueueAsync(map);

        mock.EventManager.Verify(m =>
                m.RaiseAsync(MapQueueEvents.MapQueued, It.Is<MapQueueEventArgs>(e => e.Map == map))
            , Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(100)]
    [InlineData(1000)]
    public async Task QueuedMaps_Returns_Added_Maps_In_Order_And_Correct_Count(int numMaps)
    {
        var mock = NewMapQueueServiceMock();

        var maps = new List<IMap>();
        for (var i = 0; i < numMaps; i++)
        {
            var map = new Map { Uid = $"map{i}" };
            maps.Add(map);
            await mock.MapQueueService.EnqueueAsync(map);
        }

        var queuedMaps = mock.MapQueueService.QueuedMaps;
        var mapCount = mock.MapQueueService.QueuedMapsCount;

        Assert.Equal(queuedMaps, queuedMaps);
        Assert.Equal(numMaps, mapCount);
    }

    [Fact]
    public async Task First_Added_Map_Is_Dequeued()
    {
        var mock = NewMapQueueServiceMock();
        var map1 = new Map{Uid = "map1"};
        var map2 = new Map{Uid = "map2"};
        var map3 = new Map{Uid = "map3"};

        await mock.MapQueueService.EnqueueAsync(map1);
        await mock.MapQueueService.EnqueueAsync(map2);
        await mock.MapQueueService.EnqueueAsync(map3);

        var deQueued1 = await mock.MapQueueService.DequeueNextAsync();
        var deQueued2 = await mock.MapQueueService.DequeueNextAsync();
        var deQueued3 = await mock.MapQueueService.DequeueNextAsync();
        
        Assert.Equal(map1, deQueued1);
        Assert.Equal(map2, deQueued2);
        Assert.Equal(map3, deQueued3);
    }
    
    [Fact]
    public async Task Peek_Next_Returns_First_Added_Map_Before_Dequeuing()
    {
        var mock = NewMapQueueServiceMock();
        var map1 = new Map{Uid = "map1"};
        var map2 = new Map{Uid = "map2"};
        var map3 = new Map{Uid = "map3"};

        await mock.MapQueueService.EnqueueAsync(map1);
        await mock.MapQueueService.EnqueueAsync(map2);
        await mock.MapQueueService.EnqueueAsync(map3);
        
        var map = await mock.MapQueueService.PeekNextAsync();
        
        Assert.Equal(map1, map);
    }
    
    [Fact]
    public async Task Peek_Next_Returns_First_Added_Map_After_Dequeuing()
    {
        var mock = NewMapQueueServiceMock();
        var map1 = new Map{Uid = "map1"};
        var map2 = new Map{Uid = "map2"};
        var map3 = new Map{Uid = "map3"};

        await mock.MapQueueService.EnqueueAsync(map1);
        await mock.MapQueueService.EnqueueAsync(map2);
        await mock.MapQueueService.EnqueueAsync(map3);

        await mock.MapQueueService.DequeueNextAsync();
        
        var map = await mock.MapQueueService.PeekNextAsync();
        
        Assert.Equal(map2, map);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Dropping_Map_Removes_It_From_The_Queue(int mapToDrop)
    {
        var mock = NewMapQueueServiceMock();
        IMap[] maps = [
            new Map{Uid = "map1"},
            new Map{Uid = "map2"},
            new Map{Uid = "map3"},
        ];

        foreach (var map in maps)
        {
            await mock.MapQueueService.EnqueueAsync(map);
        }
        
        await mock.MapQueueService.DropAsync(maps[mapToDrop]);
        
        Assert.Equal(2, mock.MapQueueService.QueuedMapsCount);
        Assert.DoesNotContain(maps[mapToDrop], mock.MapQueueService.QueuedMaps);
    }

    [Fact]
    public async Task Clear_Removes_Entire_Queue()
    {
        var mock = NewMapQueueServiceMock();
        
        var map1 = new Map { Uid = "map1" };
        var map2 = new Map { Uid = "map2" };
        var map3 = new Map { Uid = "map3" };

        await mock.MapQueueService.EnqueueAsync(map1);
        await mock.MapQueueService.EnqueueAsync(map2);
        await mock.MapQueueService.EnqueueAsync(map3);

        await mock.MapQueueService.ClearAsync();
        
        Assert.Equal(0, mock.MapQueueService.QueuedMapsCount);
        Assert.DoesNotContain(map1, mock.MapQueueService.QueuedMaps);
        Assert.DoesNotContain(map2, mock.MapQueueService.QueuedMaps);
        Assert.DoesNotContain(map3, mock.MapQueueService.QueuedMaps);
    }

    [Fact]
    public async Task Map_Dequeued_Raises_Event()
    {
        var mock = NewMapQueueServiceMock();

        var map = new Map();

        await mock.MapQueueService.EnqueueAsync(map);
        await mock.MapQueueService.DequeueNextAsync();

        mock.EventManager.Verify(m =>
                m.RaiseAsync(MapQueueEvents.MapDequeued, It.Is<MapQueueEventArgs>(e => e.Map == map))
            , Times.Once);
    }
    
    [Fact]
    public async Task Map_Dropped_Raises_Event()
    {
        var mock = NewMapQueueServiceMock();

        var map = new Map();

        await mock.MapQueueService.EnqueueAsync(map);
        await mock.MapQueueService.DropAsync(map);

        mock.EventManager.Verify(m =>
                m.RaiseAsync(MapQueueEvents.MapDropped, It.Is<MapQueueMapDroppedEventArgs>(e =>
                    e.Map == map
                    && e.WasNext
                ))
            , Times.Once);
    }
    
    [Fact]
    public async Task Map_Dropped_Raises_Event_And_Was_Not_Next()
    {
        var mock = NewMapQueueServiceMock();

        var map1 = new Map { Uid = "map1" };
        var map2 = new Map { Uid = "map2" };

        await mock.MapQueueService.EnqueueAsync(map1);
        await mock.MapQueueService.EnqueueAsync(map2);
        await mock.MapQueueService.DropAsync(map2);

        mock.EventManager.Verify(m =>
                m.RaiseAsync(MapQueueEvents.MapDropped, It.Is<MapQueueMapDroppedEventArgs>(e =>
                    e.Map == map2
                    && !e.WasNext
                ))
            , Times.Once);
    }
}
