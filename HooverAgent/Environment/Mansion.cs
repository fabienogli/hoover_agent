using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace HooverAgent.Environment
{
    public class Mansion : IObservable<Mansion>
    {
        private IObserver<Mansion> Observer { get; set; }
        private bool Running { get; }
        private Random Rand { get; }
        //public List<Entity> Rooms { get; }
        public Map Map { get; set; }
        private int AgentPos { get; set; }

        public Mansion(int size)
        {
            //Initializing Random once to get uniform result, and seeding to control randomness (same sequence at each run)
            Rand = new Random(1);
            //Rooms = new List<Entity>(size);
            Running = true;
            Map = new Map(size);
            InitAgent();
        }
        
        public Mansion(Mansion other)
        {
            Map = new Map(other.Map);
            AgentPos = other.AgentPos;
            //Copy performance 
        }

        private void InitAgent()
        {
            AgentPos = Rand.Next(Map.Size);
            Map.AddEntityAtPos(Entity.Agent, AgentPos);
        }

        public IDisposable Subscribe(IObserver<Mansion> observer)
        {
            Observer = observer;
            Notify();
            return null; //Maybe return an Unsubscriber
        }

        private void Notify()
        {
            Observer.OnNext(this);
        }

       /* public List<Entity> CopyRooms()
        {
            List<Entity> copy = new List<Entity>(Rooms.Count);
            foreach (var room in Rooms)
            {
                copy.Add(room);
            }

            return copy;
        }*/

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
            GenerateEntity(Entity.Dirt);
        }

        private void GenerateJewel()
        {
            GenerateEntity(Entity.Jewel);
        }

        private void GenerateEntity(Entity entity)
        {
            var randomIndex = Rand.Next(Map.Size);

            if (Map.ContainsEntityAtPos(entity, randomIndex))
            {
                //Already set so no need to reset it
                return;
            }

            Map.AddEntityAtPos(entity, randomIndex);
            Notify();
        }

        public bool HandleRequest(Action action)
        {
            int newPos = AgentPos;

            //Here we know the movement is legal and will work
            //The movement is bound to be legal because it come from the exploration, and the exploration asks
            //the environnement which actions/state are available (legal)
            switch (action)
            {
                case Action.Up:
                    newPos -= Map.SquaredSize;
                    break;
                case Action.Down:
                    newPos += Map.SquaredSize;
                    break;
                //Move down
                case Action.Left:
                    newPos--;
                    break;
                //Move left
                case Action.Right:
                    newPos++;
                    break;
            }

            Map.MoveAgent(AgentPos, newPos);
            AgentPos = newPos;
            Notify();

            return true;
        }

        public static List<State> GetSuccessors(State currentState) //@TODO
        {
            // o(n+m), n = size of mansion, m = number of actions implementation
            //todo store agentPos in state for o(m) ~= o(1)
            int agentPos = 0;
            for (int i = 0; i < currentState.Map.Size; i++)
            {
                if (currentState.Map.ContainsEntityAtPos(Entity.Agent, i))
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
            catch (IndexOutOfRangeException)
            {
                //Do nothing
            }
        }

        private static State GetNextStateForAction(State current, int agentPos, Action action)
        {
            int newPos = agentPos;
            
            switch (action)
            {
                case Action.Up:
                    newPos -= current.Map.SquaredSize;
                    break;
                case Action.Down:
                    newPos += current.Map.SquaredSize;
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

            if (newPos < 0 || newPos >= current.Map.Size)
            {
                throw new IndexOutOfRangeException();
            }

            Map map = new Map(current.Map);
            map.MoveAgent(agentPos, newPos);
            State state = new State(map, action);
            return state;
        }

        public static int GetCostForAction(Action action)
        {
            return 1;
        }
    }
}