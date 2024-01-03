using System.Collections;
using EvoSC.Modules.Official.MapQueueModule.Interfaces.Utils.AsyncDeque;

namespace EvoSC.Modules.Official.MapQueueModule.Utils.AsyncDeque;

public class AsyncDeque<TItem> : IAsyncDeque<TItem>
{
    private IDequeNode<TItem>? _last;
    private IDequeNode<TItem>? _first;
    private readonly object _lock = new();
    private int _count = 0;

    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
    {
        var node = _first;

        while (node != null)
        {
            yield return node.Item;
            node = node.Next;
        }
    }

    public IEnumerator GetEnumerator()
    {
        return GetEnumerator();
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

            _count++;
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

            _count--;
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
                    if (node == _first)
                    {
                        _first = _first.Next;
                    }
                    else if (node == _last)
                    {
                        _last = _last.Previous;
                    }
                    else
                    {
                        node.Previous.Next = node.Next;
                        node.Next.Previous = node.Previous;
                    }

                    _count--;
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
            _count = 0;
        }
    }
}
