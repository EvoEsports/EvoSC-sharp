namespace EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;

public interface IDequeNode<TItem>
{
    /// <summary>
    /// The next item in the queue.
    /// </summary>
    public IDequeNode<TItem>? Next { get; set; }
    
    /// <summary>
    /// The previous item in the queue.
    /// </summary>
    public IDequeNode<TItem>? Previous { get; set; }
    
    /// <summary>
    /// The value of the item.
    /// </summary>
    public TItem Item { get; }
}
