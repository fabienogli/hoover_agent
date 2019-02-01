using System;
using System.Collections.Generic;
using HooverAgent.Environment;

namespace HooverAgent.Search
{
    public class AStarIterator : Iterator<Tree.Node>
    {
        private Tree.Node Current { get; set; }
        private PriorityQueue<Tree.Node> Frontier { get; set; }

        public AStarIterator(Tree tree)
        {
            Frontier = new PriorityQueue<Tree.Node>();
            Frontier.Enqueue(tree.Root, 0);
        }
            
         
        public bool HasNext()
        {
            return Frontier.Any();
        }

        public Tree.Node GetNext()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No element in queue");
            }

            Current = Frontier.Dequeue();
            return Current;
        }
        
        public void Expand()
        {
            CreateAndEnqueueNodes(Current, Mansion.GetSuccessors(Current.State));
        }

        private void CreateAndEnqueueNodes(Tree.Node parent, List<State> nextPossibleStates)
        {
            nextPossibleStates.ForEach(state =>
            {
                var node = new Tree.Node(parent, state);
                parent.AddChild(node);
                int h = Mansion.GetHeuristicForState(state);
                int g = node.Cost;
                Frontier.Enqueue(node, g + h);
            });
        }
    }
}