using System.Runtime.CompilerServices;
using EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;
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

    [Fact]
    public void Count_Is_Updated_When_Enqueuing()
    {
        var queue = new AsyncDeque<MyItemType>();
        
        queue.Enqueue(new MyItemType { MyProp = "Item 1" });
        queue.Enqueue(new MyItemType { MyProp = "Item 2" });
        queue.Enqueue(new MyItemType { MyProp = "Item 3" });

        Assert.Equal(3, queue.Count);
    }
    
    [Fact]
    public void Count_Is_Updated_When_Dequeuing()
    {
        var queue = new AsyncDeque<MyItemType>();
        
        queue.Enqueue(new MyItemType { MyProp = "Item 1" });
        queue.Enqueue(new MyItemType { MyProp = "Item 2" });
        queue.Enqueue(new MyItemType { MyProp = "Item 3" });

        queue.Dequeue();
        queue.Dequeue();

        Assert.Single(queue);
        Assert.Equal(1, queue.Count);
    }

    [Fact]
    public void Clear_Removes_Everything_And_Resets_Count()
    {
        var queue = new AsyncDeque<MyItemType>();
        
        queue.Enqueue(new MyItemType { MyProp = "Item 1" });
        queue.Enqueue(new MyItemType { MyProp = "Item 2" });
        queue.Enqueue(new MyItemType { MyProp = "Item 3" });
        
        queue.Clear();
        
        Assert.Empty(queue);
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void Empty_Queue_Throws_Exception_On_Dequeue()
    {
        var queue = new AsyncDeque<MyItemType>();

        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
    }

    [Fact]
    public void PeekFirst_Returns_First_In_Queue()
    {
        var queue = new AsyncDeque<MyItemType>();
        
        queue.Enqueue(new MyItemType { MyProp = "Item 1" });
        queue.Enqueue(new MyItemType { MyProp = "Item 2" });
        queue.Enqueue(new MyItemType { MyProp = "Item 3" });

        var first = queue.PeekFirst();
        
        Assert.Equal("Item 1", first.MyProp);
    }

    [Fact]
    public void PeekFirst_Throws_If_Empty()
    {
        var queue = new AsyncDeque<MyItemType>();

        Assert.Throws<InvalidOperationException>(() => queue.PeekFirst());
    }

    [Theory]
    [InlineData(new []{0}, 2, "Item 2")]
    [InlineData(new []{1}, 2, "Item 1")]
    [InlineData(new []{2}, 2, "Item 1")]
    [InlineData(new []{0, 2}, 1, "Item 2")]
    [InlineData(new []{0, 1}, 1, "Item 3")]
    [InlineData(new []{2, 1}, 1, "Item 1")]
    [InlineData(new []{1, 0}, 1, "Item 3")]
    [InlineData(new []{2, 0}, 1, "Item 2")]
    public void Dropping_Updates_Accordingly(int[] remove, int expectedCount, string expectedFirst)
    {
        var queue = Create3ItemQueue();

        foreach (var toRemove in remove)
        {
            queue.Queue.Drop((MyItemType)((ITuple)queue)[toRemove]);
        }

        var first = queue.Queue.PeekFirst();
        
        Assert.Equal(expectedCount, queue.Queue.Count);
        Assert.Equal(expectedFirst, first.MyProp);
    }

    [Fact]
    public void Dropping_Non_Existent_Item_Throws_Exception()
    {
        var queue = new AsyncDeque<MyItemType>();

        var item = new MyItemType();

        Assert.Throws<InvalidOperationException>(() => queue.Drop(item));
    }

    [Fact]
    public void CopyTo_Is_Not_Supported()
    {
        var queue = new AsyncDeque<MyItemType>();
        Assert.Throws<NotSupportedException>(() => queue.CopyTo(Array.Empty<int>(), 0));
    }
    
    private (
        MyItemType Item1,
        MyItemType Item2,
        MyItemType Item3,
        IAsyncDeque<MyItemType> Queue
        ) Create3ItemQueue()
    {
        var queue = new AsyncDeque<MyItemType>();

        var item1 = new MyItemType { MyProp = "Item 1" };
        var item2 = new MyItemType { MyProp = "Item 2" };
        var item3 = new MyItemType { MyProp = "Item 3" };
        
        queue.Enqueue(item1);
        queue.Enqueue(item2);
        queue.Enqueue(item3);

        return (item1, item2, item3, queue);
    }
}
