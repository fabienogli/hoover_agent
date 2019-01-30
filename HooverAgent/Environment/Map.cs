using System;
using System.Collections.Generic;

namespace HooverAgent.Environment
{
    public class Map
    {
        private List<Entity> Rooms { get; }
        
        public int Size => Rooms.Capacity;

        public int SquaredSize => (int) Math.Sqrt(Size);

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
            Rooms[pos] |= flag;
        }

        public void RemoveEntityAtPos(Entity flag, int pos)
        {
            Rooms[pos] &= ~flag;
        }

        public bool ContainsEntityAtPos(Entity flag, int pos)
        {
            return Rooms[pos].HasFlag(flag);
        }

        public void MoveAgent(int from, int to)
        {
            var agent = Entity.Agent;
            RemoveEntityAtPos(agent, from);
            AddEntityAtPos(agent, to);
        }

        public Entity GetEntityAt(int pos)
        {
            return Rooms[pos];
        } 
    }
}