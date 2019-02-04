using System;
using System.Collections.Generic;

namespace HooverAgent.Search
{
    public class PriorityQueue<T>
    {
        private List<Tuple<T, double>> Queue;
        
        public int Count => Queue.Count;

        public PriorityQueue()
        {
            Queue = new List<Tuple<T, double>>();
        }
        public void Enqueue(T value, double priority)
        {
            for (var i = 0; i < Queue.Count; i++)
            {
                if (priority < Queue[i].Item2)
                {
                    Queue.Insert(i, new Tuple<T, double>(value, priority));
                    return;
                }
            }
            Queue.Add(new Tuple<T, double>(value, priority));
         }

        public T Dequeue()
        {
            if (Queue.Count == 0)
            {
                throw new InvalidOperationException("No more element in the queue !");
            }
            var head = Queue[0];
            Queue.RemoveAt(0);
            return head.Item1;
        }

        public bool Any()
        {
            return Queue.Count > 0;
        }

    }
}