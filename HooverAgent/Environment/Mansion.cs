using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Action = HooverAgent.Agent.Action;

namespace HooverAgent.Environment
{
    public class Mansion : IObservable<Mansion>
    {
        private Random _rand;
        private IObserver<Mansion> _observer;
        private int AgentPos { get; set; }

        public Mansion(int size)
        {
            Rand = new Random(1);
            Rooms = new List<Entities>(size);
            for (var i = 0; i < size; i++)
            {
                Rooms.Add(Entities.Nothing);
                //Generate jewels and dirt on first run 
            }

            Running = true;
            AgentPos = Rand.Next(size);
            Rooms[AgentPos] |= Entities.Agent;
            //Initializing Random once to get uniform result, and seeding to control randomness

        }

        public Mansion(Mansion other)
        {
            Rooms = other.CopyRooms();
            //Copy agent state (pos) 
            //Copy performance 
        }

        public List<Entities> CopyRooms()
        {
            List<Entities> copy = new List<Entities>(Rooms.Count);
            foreach (var room in Rooms)
            {
                copy.Add(room);
            }

            return copy;
        } 

        private bool Running { get; }

        private Random Rand { get; }

        public List<Entities> Rooms { get; }

        public void Run()
        {
            
            while (Running)
            {
                if (ShouldGenerateDirt())
                {
                    GenerateDirt();
                }

                if (ShouldGenerateJewel())
                {
                    GenerateJewel();
                }
                Thread.Sleep(10);
            }
        }

        private bool ShouldGenerateDirt()
        {
            //If some proba
            //@todo : Generate with timer => Use StopWatch i.e : Every n seconds, there is a probability p of generating dirt
            //todo : Verify dirt distribution (should not fill all map)

            return Rand.Next(100) < 5;
        }

        private bool ShouldGenerateJewel()
        {
            //If some proba
            return Rand.Next(100) < 5;
        }
        
        private void GenerateDirt()
        {
            GenerateObject(Entities.Dirt);
        }
        
        private void GenerateJewel()
        {
            GenerateObject(Entities.Jewel);
        }
        
        private void GenerateObject(Entities obj)
        {
            int randomIndex = Rand.Next(Rooms.Count);
            Entities el = Rooms[randomIndex];

            if (el.HasFlag(obj))
            {
                //Already set so no need to reset it
                return;
            }

            Rooms[randomIndex] |= obj;
            _observer.OnNext(this);
        }
        public IDisposable Subscribe(IObserver<Mansion> observer)
        {
            _observer = observer;
            _observer.OnNext(this);
            return null; //Maybe return an Unsubscriber
        }

        public bool HandleRequest(Action action)
        {
            
            State state = new State(Rooms, Action.Idle);
            State next = GetNextStateForAction(state, AgentPos, action);
            int size = (int) Math.Sqrt(Rooms.Count);
            int newPos = AgentPos;
            
            //Here we assume the movement is legal and will work (no out of bounds treated)
            //todo maybe change this assumption...
            switch (action)
            {
                case Action.Up:
                    newPos -= size;
                    break;
                case Action.Down:
                    newPos += size;
                    break;
                    //Move down
                case Action.Left:
                    newPos--;
                    break;
                    //Move left
                case Action.Right:
                    newPos++;
                    break;
                default:
                    break;
            }

            
            //todo Maybe lock grid here
            Rooms[AgentPos] &= ~Entities.Agent;
            Rooms[newPos] |= Entities.Agent;
            AgentPos = newPos;
            _observer.OnNext(this);
            
            return true;
            
        }

        public static List<State> GetSuccessors(State currentState) //@TODO
        {
            // o(n+m), n = size of mansion, m = number of actions implementation
            //todo store agentPos in state for o(m) ~= o(1)
            int agentPos = 0;
            for (int i = 0; i < currentState.GetMap().Count; i++)
            {
                if (currentState.GetMap()[i].HasFlag(Entities.Agent))
                {
                    agentPos = i;
                    break;
                }
            }

            List<State> successors = new List<State>();
            AddStateForActionIfValid(successors, currentState, agentPos, Action.Up);
            AddStateForActionIfValid(successors, currentState, agentPos, Action.Down);
            AddStateForActionIfValid(successors, currentState, agentPos, Action.Left);
            AddStateForActionIfValid(successors, currentState, agentPos, Action.Right);

            return successors;
        }

        private static void AddStateForActionIfValid(List<State> successors, State current, int agentPos, Action action)
        {
            try
            {
                State state = GetNextStateForAction(current, agentPos, action);
                successors.Add(state);
            }
            catch (IndexOutOfRangeException e)
            {
                //Do nothing
            }
        }

        private static State GetNextStateForAction(State current, int agentPos, Action action)
        {
            int newPos = agentPos;
            int nTiles = current.GetMap().Count;
            int size = (int) Math.Sqrt(nTiles);
            switch (action)
            {
                case Action.Up:
                    newPos -= size;
                    break;
                case Action.Down:
                    newPos += size;
                    break;
                //Move down
                case Action.Left:
                    newPos--;
                    break;
                //Move left
                case Action.Right:
                    newPos++;
                    break;
                default:
                    throw new InvalidDataException();
            }

            if (newPos < 0 || newPos >= nTiles)
            {
                throw new IndexOutOfRangeException();
            }
            
            List<Entities> copy = new List<Entities>(nTiles);
            foreach (var entity in current.GetMap())
            {
                copy.Add(entity);
            }

            copy[agentPos] &= ~Entities.Agent;
            copy[newPos] |= Entities.Agent;
            
            State state = new State(copy, action);
            return state;
        }

        public static int GetCostForAction(Action action)
        {
            return 1;
        }
    }
}


