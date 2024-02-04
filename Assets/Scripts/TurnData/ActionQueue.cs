using System.Collections.Generic;

namespace TurnData
{
    public class ActionQueue<T>
    {
        private readonly LinkedList<T> _items;

        public ActionQueue()
        {
            _items = new LinkedList<T>();
        }

        public int Count => _items.Count;

        public void Enqueue(T element)
        {
            _items.AddLast(element);
        }
        
        public void EnqueueAtFront(T element)
        {
            _items.AddFirst(element);
        }

        public T Dequeue()
        {
            var first = _items.First;
            _items.RemoveFirst();
            
            return first.Value;
        }

        public T Peek()
        {
            return _items.First.Value;
        }
    }
}