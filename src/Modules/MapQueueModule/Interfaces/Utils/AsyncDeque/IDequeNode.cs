namespace EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;

public interface IDequeNode<TItem>
{
    public IDequeNode<TItem>? Next { get; set; }
    public IDequeNode<TItem>? Previous { get; set; }
    
    public TItem Item { get; }
}
