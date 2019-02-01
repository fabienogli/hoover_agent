using System;
using System.Collections.Generic;
using HooverAgent.Environment;

namespace HooverAgent.Search
{
    public class Tree
    {
        public class Node
        {
            private List<Node> Children { get; } 
            public Node Parent { get; }
            public State State { get; }
            
            public int Depth { get; }
            
            public int Cost { get; }

            public Node(State state)
            {
                Depth = 0;
                State = state;
                Cost = Mansion.GetCostForAction(state.Action);
                Children = new List<Node>();
            }

            public Node(Node parent, State state) : this(state)
            {
                Parent = parent;
                Cost = parent.Cost + Mansion.GetCostForAction(state.Action);
                Depth = parent.Depth + 1;
            }
           
            public void AddChild(Node child)
            {
                Children.Add(child);
            }
        }

        public Node Root { get; }

        public Tree(Node root)
        {
            Root = root;
        }
            
    }
}