using System;
using System.Collections.Generic;
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
       // private int AgentPos { get; set; }

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
            //Copy performance 
        }

        private void InitAgent()
        {
            var agentPos = Rand.Next(Map.Size);
            Map.AddEntityAtPos(Entity.Agent, agentPos);
            Map.AgentPos = agentPos;
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

                Thread.Sleep(50);
            }
        }

        private bool ShouldGenerateDirt()
        {
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
            Map.ApplyAction(action);
            Notify();
            return true;
        }

        public static List<State> GetSuccessors(State currentState) //@TODO
        {
            var successors = new List<State>();
            var values = Enum.GetValues(typeof(Action));
            foreach (Action action in values)
            {
                //todo tmp, just because I don't want to deal with this for testing purposes
                if(action == Action.Pick || action == Action.Snort) 
                    continue;
                
                
                AddStateForActionIfValid(successors, currentState, action);
            }
            
            return successors;
        }
        
        private static void AddStateForActionIfValid(ICollection<State> successors, State current, Action action)
        {
            try
            {
                var state = GetNextStateForAction(current, action);
                successors.Add(state);
            }
            catch (IndexOutOfRangeException)
            {
                //Do nothing
            }
        }

        private static State GetNextStateForAction(State current, Action action)
        {
            var map = new Map(current.Map);
            map.ApplyAction(action);
            var state = new State(map, action);
            return state;
        }

        public static int GetCostForAction(Action action)
        {
            return 1;
        }
    }
}