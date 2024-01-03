using EvoSC.Modules.Official.MapQueueModule.Utils.AsyncDeque;

namespace MapQueueModuleTests.Utils;

public class AsyncDequeTests
{
    class MyItemType
    {
        public string MyProp { get; set; }
    }
    
    [Fact]
    public void Item_Is_Added_And_Dequeued()
    {
        var queue = new AsyncDeque<MyItemType>();

        queue.Enqueue(new MyItemType { MyProp = "Hello" });

        var item = queue.Dequeue();

        Assert.Equal("Hello", item.MyProp);
    }

    [Fact]
    public void Multiple_Items_Are_Added_And_Dequeued_In_FIFO()
    {
        var queue = new AsyncDeque<MyItemType>();
        
        queue.Enqueue(new MyItemType { MyProp = "Item 1" });
        queue.Enqueue(new MyItemType { MyProp = "Item 2" });
        queue.Enqueue(new MyItemType { MyProp = "Item 3" });

        var first = queue.Dequeue();
        var second = queue.Dequeue();
        var third = queue.Dequeue();
        
        Assert.Equal("Item 1", first.MyProp);
        Assert.Equal("Item 2", second.MyProp);
        Assert.Equal("Item 3", third.MyProp);
    }
}
