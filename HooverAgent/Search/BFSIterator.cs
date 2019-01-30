using System.Collections.Generic;
using System.Linq;

namespace HooverAgent.Search
{
    public class BFSIterator : Iterator<Tree.Node>
    {
        private Tree _tree;
        private Queue<Tree.Node> Frontier;
        
        
        public bool HasNext()
        {
            return Frontier.Any();
        }

        public Tree.Node GetNext()
        {
            Tree.Node current = Frontier.Dequeue();
            current.Parent.
        }
    }
}