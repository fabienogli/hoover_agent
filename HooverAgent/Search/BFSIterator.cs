using System.Collections.Generic;
using System.Linq;
using HooverAgent.Environment;

namespace HooverAgent.Search
{
    public class BFSIterator : Iterator<Tree.Node>
    {
        private Queue<Tree.Node> Frontier;
        private Mansion Environment; //@TODO static method to get env
        
        public bool HasNext()
        {
            return Frontier.Any();
        }

        public Tree.Node GetNext()
        {
            Tree.Node current = Frontier.Dequeue();
            CreateNodes(Environment.GetNextFromState(current.State)).ForEach(node => Frontier.Enqueue(node));
            return current;
        }

        private List<Tree.Node> CreateNodes(List<State> getNextFromState)
        {
            throw new System.NotImplementedException();
        }
    }
}