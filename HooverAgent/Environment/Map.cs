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
        public int AgentPos { get; set; }

        public int DirtCounter { get; private set;  }
        public int JewelCounter { get; private set;  }

        private object _lock = new object();  
        public Map(int size)
        {
            Rooms = new List<Entity>(size);
            DirtCounter = 0;
            JewelCounter = 0;
            Init();
        }

        public Map(Map other)
        {
            Rooms = new List<Entity>(other.Size);
            lock (other._lock)
            {
                foreach (var room in other.Rooms)
                {
                    Rooms.Add(room);
                }

                AgentPos = other.AgentPos;
                DirtCounter = other.DirtCounter;
                JewelCounter = other.JewelCounter;
            }
           
        }

        private void Init()
        {
            Random rand = new Random(2);
            for (var i = 0; i < Rooms.Capacity; i++)
            {
                Rooms.Add(Entity.Nothing);
                if (rand.Next(100) < 30)
                {
                    AddEntityAtPos(Entity.Dirt, i);
                    DirtCounter++;
                }
                
            }
        }

        public void AddEntityAtPos(Entity flag, int pos)
        {
            lock (_lock)
            {
                Rooms[pos] |= flag; 
            }

            switch (flag)
            {
                case Entity.Dirt:
                    DirtCounter++;
                    break;
                case Entity.Jewel:
                    JewelCounter++;
                    break;
            }
            
        }

        public void RemoveEntityAtPos(Entity flag, int pos)
        {
            lock (_lock)
            {
                if (ContainsEntityAtPos(flag,pos))
                {
                    Rooms[pos] &= ~flag;
                    switch (flag)
                    {
                        case Entity.Dirt:
                            DirtCounter--;
                            break;
                        case Entity.Jewel:
                            JewelCounter--;
                            break;
                    }
                }
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
                    if (newPos % SquaredSize == 0)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    newPos--;
                    
                    break;
                case Action.Right:
                    
                    if (newPos % SquaredSize == SquaredSize-1)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    newPos++;
                    break;
                case Action.Snort:
                    RemoveEntityAtPos(Entity.Dirt, newPos);
                    RemoveEntityAtPos(Entity.Jewel, newPos);
                    return;
                case Action.Pick:
                    RemoveEntityAtPos(Entity.Jewel, newPos);
                    return;
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