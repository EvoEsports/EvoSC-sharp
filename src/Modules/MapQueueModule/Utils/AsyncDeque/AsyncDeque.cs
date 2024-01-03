using System.Collections;
using EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;

namespace EvoSC.Modules.Official.MapQueueModule.Utils.AsyncDeque;

public class AsyncDeque<TItem> : IAsyncDeque<TItem>
{
    private IDequeNode<TItem>? _last;
    private IDequeNode<TItem>? _first;
    private readonly object _lock = new();
    private int _count = 0;
    
    public IEnumerator GetEnumerator()
    {
        var node = _first;

        while (node != null)
        {
            yield return node.Item;
            node = node.Next;
        }
    }

    public void CopyTo(Array array, int index)
    {
        throw new NotSupportedException();
    }

    public int Count
    {
        get
        {
            lock (SyncRoot)
            {
                return _count;
            }
        }
    }

    public bool IsSynchronized => true;
    public object SyncRoot => _lock;
    

    public void Enqueue(TItem item)
    {
        lock (SyncRoot)
        {
            if (_last == null)
            {
                _last = new DequeNode<TItem> { Next = null, Previous = null, Item = item };
                _first = _last;
            }
            else
            {
                var newItem = new DequeNode<TItem> { Next = null, Previous = _last, Item = item };
                _last.Next = newItem;
                _last = newItem;
            }
        }
    }

    public TItem Dequeue()
    {
        lock (SyncRoot)
        {
            if (_first == null)
            {
                throw new InvalidOperationException("There are no items in the queue");
            }

            var item = _first.Item;
            
            if (_first == _last)
            {
                _first = null;
                _last = null;
            }
            else
            {
                _first = _first.Next;
            }

            return item;
        }
    }

    public TItem PeekFirst()
    {
        lock (SyncRoot)
        {
            if (_first == null)
            {
                throw new InvalidOperationException("There are no items in the queue");
            }

            return _first.Item;
        }
    }

    public void Drop(TItem item)
    {
        lock (SyncRoot)
        {
            var node = _first;

            while (node != null)
            {
                if (node.Item != null && node.Item.Equals(item))
                {
                    if (node.Previous != null)
                    {
                        node.Previous.Next = node.Next;
                    }

                    if (node.Next != null)
                    {
                        node.Next.Previous = node.Previous;
                    }

                    return;
                }
            
                node = node.Next;
            }

            throw new InvalidOperationException("Item does not exist in the queue");
        }
    }

    public void Clear()
    {
        lock (SyncRoot)
        {
            _first = null;
            _last = null;
        }
    }
}
