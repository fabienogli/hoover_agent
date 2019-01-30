using System;
using System.Collections.Generic;
using Action = HooverAgent.Agent.Action;

namespace HooverAgent.Environment
{
     
    public class State
    {
        public Tuple<List<Entities>, Action> Current { get; }

        public Action GetAction()
        {
            return Current.Item2;
        }

        public List<Entities> GetMap()
        {
            return Current.Item1;
        }

        public State(List<Entities> entities, Action action)
        {
            Current = new Tuple<List<Entities>, Action>(entities, action);
        }
    }
}