using System;
using System.Collections.Generic;
using HooverAgent.Environment;

namespace HooverAgent.Search
{
    public class Tree
    {
        public class Node
        {
            private List<Node> Children;
            private State State;
            private Action _action;

            public Node(Node parent)
            {
                Parent = parent;
                Children = new List<Node>();
            }
            
            public void AddChild(Node child)
            {
                Children.Add(child);
            }
            
            public Action Action
            {
                get => _action;
                set => _action = value;
            }

            public Node Parent { get; }
            
        }

        private Node Root;
        private Mansion Environment;

        public Tree(Node root, Mansion environment)
        {
            Root = root;
            Environment = environment;
        }
            
    }
}