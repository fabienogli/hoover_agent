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
            public State State { get; set; }
            
            public int Depth { get; }
            
            public int Cost { get;  }

            public Node(Node parent, State state)
            {
                Parent = parent;
                Children = new List<Node>();
                Depth = parent.Depth + 1;
                State = state;
                Cost = Mansion.GetCostForAction(state.GetAction());
            }

            public Node(State state)
            {
                Depth = 0;
                State = state;
                Cost = Mansion.GetCostForAction(state.GetAction());
                Children = new List<Node>();
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