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

        public int TotalDirtCounter { get; private set;  }
        public int TotalJewelCounter { get; private set;  }
        public int SnortedDirtCounter { get; private set;  }
        public int SnortedJewelCounter { get; private set;  }
        public int PickedJewelCounter { get; private set;  }

        private object _lock = new object();  
        public Map(int size)
        {
            Rooms = new List<Entity>(size);
            InitCounters();
            Init();
        }

        private void InitCounters()
        {
            TotalDirtCounter = 0;
            TotalJewelCounter = 0;
            SnortedDirtCounter = 0;
            SnortedJewelCounter = 0;
            PickedJewelCounter = 0;
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
                CopyCounter(other);
            }
           
        }

        private void CopyCounter(Map other)
        {
            TotalDirtCounter = other.TotalDirtCounter;
            TotalJewelCounter = other.TotalJewelCounter;
            SnortedDirtCounter = other.SnortedDirtCounter;
            SnortedJewelCounter = other.SnortedJewelCounter;
            PickedJewelCounter = other.PickedJewelCounter;
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
                    TotalDirtCounter++;
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
//            Console.WriteLine("Dans update Counter");
            lock(_lock)
            {
                switch (action)
                {
                    case Action.Snort:
                        
                        if (ContainsEntityAtPos(Entity.Dirt, AgentPos))
                        {
                            Console.WriteLine("snort dust augmented: " + SnortedDirtCounter);
                            SnortedDirtCounter++;
                        }
                        if (ContainsEntityAtPos(Entity.Jewel, AgentPos))
                        {
                            Console.WriteLine("snort jewel counter augmented: " + SnortedJewelCounter);
                            SnortedJewelCounter++;
                        }
                        break;
                    case Action.Pick:
                        if (ContainsEntityAtPos(Entity.Jewel, AgentPos))
                        {
                            Console.WriteLine("picked jewel counter augmented: " + SnortedJewelCounter);
                            PickedJewelCounter++;
                        }
                        break;
                }
            }
        }
    }
}