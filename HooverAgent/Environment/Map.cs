using System;
using System.Collections.Generic;
using System.IO;

namespace HooverAgent.Environment
{
    public class Map
    {
        private List<Entity> Rooms { get; }
        
        public int Size => Rooms.Capacity;

        public int SquaredSize => (int) Math.Sqrt(Size);
        
        //Storing AgentPos to avoid computation
        public int AgentPos { private get; set; }

        private object _lock = new object();  
        public Map(int size)
        {
            Rooms = new List<Entity>(size);
            Init();
        }

        public Map(Map other)
        {
            Rooms = new List<Entity>(other.Size);
            foreach (var room in other.Rooms)
            {
                Rooms.Add(room);
            }

            AgentPos = other.AgentPos;
        }

        private void Init()
        {
            for (var i = 0; i < Rooms.Capacity; i++)
            {
                Rooms.Add(Entity.Nothing);
            }
        }

        public void AddEntityAtPos(Entity flag, int pos)
        {
            lock (_lock)
            {
                Rooms[pos] |= flag;    
            }
        }

        public void RemoveEntityAtPos(Entity flag, int pos)
        {
            lock (_lock)
            {
                Rooms[pos] &= ~flag;    
            }
        }

        public bool ContainsEntityAtPos(Entity flag, int pos)
        {
            return Rooms[pos].HasFlag(flag);
        }

        public void MoveAgentTo(int pos)
        {
            var agent = Entity.Agent;
            lock (_lock)
            {
                RemoveEntityAtPos(agent, AgentPos);
                AddEntityAtPos(agent, pos);    
            }
        }

        public Entity GetEntityAt(int pos)
        {
            lock (_lock)
            {
                return Rooms[pos];    
            }
        }

        public void ApplyAction(Action action)
        {
            var newPos = AgentPos;
            switch (action)
            {
                case Action.Idle:
                    return;
                case Action.Up:
                    newPos -= SquaredSize;
                    break;
                case Action.Down:
                    newPos += SquaredSize;
                    break;
                case Action.Left:
                    newPos--;
                    break;
                case Action.Right:
                    newPos++;
                    break;
                default:
                    throw new InvalidDataException("No such action was implemented !");
            }

            if (newPos < 0 || newPos >= Size)
            {
                throw new IndexOutOfRangeException();
            }

            MoveAgentTo(newPos);
            AgentPos = newPos;
        }   
    }
}