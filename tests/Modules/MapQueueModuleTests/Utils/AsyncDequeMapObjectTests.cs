using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using EvoSC.Modules.Official.MapQueueModule.Utils.AsyncDeque;

namespace MapQueueModuleTests.Utils;

public class AsyncDequeMapObjectTests
{
    [Fact]
    public void Map_Added_And_Dequeued()
    {
        var queue = new AsyncDeque<IMap>();
        var map = new Map { Uid = "AABt6LZudMynw1HFC8U181NszFh", Name = "Something" };
        
        queue.Enqueue(map);

        var dequeuedMap = queue.Dequeue();
        
        Assert.NotNull(dequeuedMap);
        Assert.Equal("AABt6LZudMynw1HFC8U181NszFh", dequeuedMap.Uid);
        Assert.Equal("Something", dequeuedMap.Name);
    }
    
    [Fact]
    public void Map_Added_And_Dropped_With_New_MapInstance()
    {
        var queue = new AsyncDeque<IMap>();
        var map = new Map { Uid = "AABt6LZudMynw1HFC8U181NszFh", Name = "Something" };
        var mapNewInstance = new Map { Uid = "AABt6LZudMynw1HFC8U181NszFh", Name = "Something" };
        
        queue.Enqueue(map);
        queue.Drop(mapNewInstance);

        Assert.Equal(0, queue.Count);
    }
}
