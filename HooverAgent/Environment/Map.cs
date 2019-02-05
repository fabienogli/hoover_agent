using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HooverAgent.Environment
{
    public class Map
    {
        private List<Entity> Rooms { get; }
        
        public int Size => Rooms.Capacity;

        public int SquaredSize => (int) Math.Sqrt(Size);
        
        //Storing AgentPos to avoid computation
        public int AgentPos { get; set; }

        public double TotalDirtCounter { get; private set;  }
        public double TotalJewelCounter { get; private set;  }
        public double SnortedDirtCounter { get; private set;  }
        public double SnortedJewelCounter { get; private set;  }
        public double PickedJewelCounter { get; private set;  }

        private object _lock = new object();  
        public Map(int size)
        {
            Rooms = new List<Entity>(size);
            InitCounters();
            InitEntities();
        }

        private void InitCounters()
        {
            TotalDirtCounter = 0;
            TotalJewelCounter = 0;
            SnortedDirtCounter = 0;
            SnortedJewelCounter = 0;
            PickedJewelCounter = 0;
        }
        
        private void InitEntities()
        {
            Random rand = new Random(2);
            for (var i = 0; i < Rooms.Capacity; i++)
            {
                Rooms.Add(Entity.Nothing);
                if (rand.Next(100) < 30)
                {
                    AddEntityAtPos(Entity.Dirt, i);
                    TotalDirtCounter++;
                }
                
            }
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
                CopyCounters(other);
            }
           
        }

        private void CopyCounters(Map other)
        {
            TotalDirtCounter = other.TotalDirtCounter;
            TotalJewelCounter = other.TotalJewelCounter;
            SnortedDirtCounter = other.SnortedDirtCounter;
            SnortedJewelCounter = other.SnortedJewelCounter;
            PickedJewelCounter = other.PickedJewelCounter;
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
                    TotalDirtCounter++;
                    break;
                case Entity.Jewel:
                    TotalJewelCounter++;
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
                    UpdateCounter(action);
                    RemoveEntityAtPos(Entity.Dirt, newPos);
                    RemoveEntityAtPos(Entity.Jewel, newPos);
                    return;
                case Action.Pick:
                    UpdateCounter(action);
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

        private void UpdateCounter(Action action)
        {
            lock(_lock)
            {
                switch (action)
                {
                    case Action.Snort:
                        
                        if (ContainsEntityAtPos(Entity.Dirt, AgentPos))
                        {
                            SnortedDirtCounter++;
                        }
                        if (ContainsEntityAtPos(Entity.Jewel, AgentPos))
                        {
                            SnortedJewelCounter++;
                        }
                        break;
                    case Action.Pick:
                        if (ContainsEntityAtPos(Entity.Jewel, AgentPos))
                        {
                            PickedJewelCounter++;
                        }
                        break;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                sb.Append(EntityStringer.ObjectToString(GetEntityAt(i)));
            }

            return sb.ToString();
        }
    }
}