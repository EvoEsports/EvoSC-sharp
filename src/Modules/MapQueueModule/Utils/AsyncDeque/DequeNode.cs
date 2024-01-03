using EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;

namespace EvoSC.Modules.Official.MapQueueModule.Utils.AsyncDeque;

public class DequeNode<TItem> : IDequeNode<TItem>
{
    public IDequeNode<TItem>? Next { get; set; }
    public IDequeNode<TItem>? Previous { get; set; }
    public TItem Item { get; init; }
}
