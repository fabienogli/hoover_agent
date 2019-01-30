using System;
using System.Collections.Generic;
using System.Linq;
using HooverAgent.Environment;

namespace HooverAgent.Search
{
    public class BFSIterator : Iterator<Tree.Node>
    {
        private Queue<Tree.Node> Frontier;

        private Tree.Node Current { get; set; }
        public BFSIterator(Tree tree)
        {
            Frontier = new Queue<Tree.Node>();
            Frontier.Enqueue(tree.Root);
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
                Frontier.Enqueue(node);
            });
        }
    }
}