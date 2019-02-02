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
        public Map Map { get; }

        private int _fitness;
        
        public int Fitness => _fitness;

        private const double MaxDirtCoverage = 0.25;
        private const double MaxJewelCoverage = 0.25;

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
            _fitness = other._fitness;
        }

        private void InitAgent()
        {
            var agentPos = Rand.Next(Map.Size);
            Map.AddEntityAtPos(Entity.Agent, agentPos);
            Map.AgentPos = agentPos;
            _fitness = 0;
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
            UpdateFitness(action);
            Map.ApplyAction(action);
            Notify();
            return true;
        }

        private void UpdateFitness(Action action)
        {
            switch (action)
            {
                case Action.Up: 
                    //fallthrough
                case Action.Down:
                    //fallthrough
                case Action.Left:
                    //fallthough
                case Action.Right:
                    _fitness += (int) Performances.Move;
                    break;
                
                case Action.Snort:
                    if (Map.ContainsEntityAtPos(Entity.Jewel, Map.AgentPos))
                    {
                        _fitness += (int) Performances.SnortJewel;
                    }

                    if (Map.ContainsEntityAtPos(Entity.Dirt, Map.AgentPos))
                    {
                        _fitness += (int) Performances.SnortDirt;
                    }

                    if (Map.GetEntityAt(Map.AgentPos) == Entity.Nothing)
                    {
                        _fitness += (int) Performances.PickNothing;
                    } 
                    break;
                case Action.Pick:
                    if (Map.ContainsEntityAtPos(Entity.Jewel, Map.AgentPos))
                    {
                        _fitness += (int) Performances.PickJewel;
                    }

                    if (Map.ContainsEntityAtPos(Entity.Dirt, Map.AgentPos) 
                       || Map.GetEntityAt(Map.AgentPos) == Entity.Nothing )
                    {
                        _fitness += (int) Performances.PickNothing;
                    }
                    
                    break;
            }
        }

        public static List<State> GetSuccessors(State currentState) //@TODO
        {
            var successors = new List<State>();
            var values = Enum.GetValues(typeof(Action));
            foreach (Action action in values)
            {
                
                if(action == Action.Idle)
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

        public static int GetHeuristicForState(State state)
        {

            return state.Map.DirtCounter + state.Map.JewelCounter;
        }
    }
}