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
        public Map Map { get; }

        public int Fitness { get; set; }


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
            Fitness = other.Fitness;
        }

        private void InitAgent()
        {
            var agentPos = Rand.Next(Map.Size);
            Map.AddEntityAtPos(Entity.Agent, agentPos);
            Map.AgentPos = agentPos;
            Fitness = 0;
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
            double remainingDirt = Map.TotalDirtCounter - Map.SnortedDirtCounter;
            if (remainingDirt / Map.Size < MaxDirtCoverage)
            {
                return Rand.Next(100) < 15;
            }

            return false;
        }

        private bool ShouldGenerateJewel()
        {
            double remainingJewels = Map.TotalJewelCounter - Map.PickedJewelCounter - Map.SnortedJewelCounter;
            if (remainingJewels / Map.Size < MaxJewelCoverage)
            {
                return Rand.Next(100) < 15;
            }

            return false;
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

        public bool HandleMovement(Action action)
        {
            Fitness += (int) Performances.Move;
            ApplyAndNotify(action);
            return true;
        }

        public bool HandleSnort()
        {
            if (Map.ContainsEntityAtPos(Entity.Jewel, Map.AgentPos))
            {
                Fitness += (int) Performances.SnortJewel;
            }

            if (Map.ContainsEntityAtPos(Entity.Dirt, Map.AgentPos))
            {
                Fitness += (int) Performances.SnortDirt;
            }

            if (Map.GetEntityAt(Map.AgentPos) == Entity.Nothing)
            {
                Fitness += (int) Performances.PickNothing;
            }

            ApplyAndNotify(Action.Snort);
            return true;
        }

        public bool HandlePick()
        {
            if (Map.ContainsEntityAtPos(Entity.Jewel, Map.AgentPos))
            {
                Fitness += (int) Performances.PickJewel;
            }

            if (Map.ContainsEntityAtPos(Entity.Dirt, Map.AgentPos)
                || Map.GetEntityAt(Map.AgentPos) == Entity.Nothing)
            {
                Fitness += (int) Performances.PickNothing;
            }
            ApplyAndNotify(Action.Pick);
            return true;
        }

        private void ApplyAndNotify(Action action)
        {
            Map.ApplyAction(action);
            Notify();
        }

        public static List<State> GetSuccessors(State currentState)
        {
            var successors = new List<State>();
            var values = Enum.GetValues(typeof(Action));
            foreach (Action action in values)
            {
                if (action == Action.Idle)
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

        public static double GetHeuristicForState(State state)
        {
            var alpha = 1;
            var beta = 1.5;
            var gamma = 10;

            var x = Math.Abs(state.Map.TotalDirtCounter) < 0.0001
                ? 0
                : state.Map.SnortedDirtCounter / state.Map.TotalDirtCounter;
            var y = Math.Abs(state.Map.TotalJewelCounter) < 0.0001
                ? 0
                : state.Map.PickedJewelCounter / state.Map.TotalJewelCounter;
            var z = Math.Abs(state.Map.TotalJewelCounter) < 0.0001
                ? 1
                : state.Map.SnortedJewelCounter / state.Map.TotalJewelCounter;

            var result = alpha * (1 - x) + beta * (1 - y) + gamma * z;
            return result;
        }
    }
}