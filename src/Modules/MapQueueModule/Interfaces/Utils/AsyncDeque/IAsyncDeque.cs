using System.Collections;

namespace EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;

public interface IAsyncDeque<TItem> : ICollection
{
    public void Enqueue(TItem item);
    public TItem Dequeue();
    public TItem PeekFirst();
    public void Drop(TItem item);
    public void Clear();
}
