using System;
using System.Collections.Generic;
using HooverAgent.Environment;

namespace HooverAgent.Search
{
    public class Tree
    {
        public class Node
        {
            private List<Node> _children;
            private State _state;
            private Action _action;

            public Node(Node parent)
            {
                Parent = parent;
                _children = new List<Node>();
            }
            
            public void AddChild(Node child)
            {
                _children.Add(child);
            }
            
            public Action Action
            {
                get => _action;
                set => _action = value;
            }

            public Node Parent { get; }
            
            public State State { get; }
            
            
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