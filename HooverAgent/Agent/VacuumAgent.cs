using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using HooverAgent.Environment;
using HooverAgent.Search;
using Action = HooverAgent.Environment.Action;

namespace HooverAgent.Agent
{
    public class VacuumAgent
    {
        private const string OptimalFile = "optimal";
        public const int MaxDepth = 5;
        private RoomSensor RoomSensor { get; }
        private PerformanceSensor PerformanceSensor { get; }

        private Dictionary<Action, Effector> Effectors { get; }

        private Mansion Environment { get; }

        private Map Beliefs { get; set; }

        private int Performance { get; set; }
        private Queue<Action> Intents { get; }

        public int OptimalSequenceLength { private get; set; }

        private int ActionDone { get; set; }

        private bool IsInformed { get; }
        public VacuumAgent(Mansion environment, bool informed)
        {
            Environment = environment;
            RoomSensor = new RoomSensor();
            
            PerformanceSensor = new PerformanceSensor();
            Intents = new Queue<Action>();
            OptimalSequenceLength = ReadOptimalFromFileOrDefault(MaxDepth);
            ActionDone = 0;
            IsInformed = informed;
            Effectors = EffectorFactory.GetActionEffectorDictionary(environment);
        }

        private static int ReadOptimalFromFileOrDefault(int defaultValue)
        {
            try
            {
                using (TextReader reader = File.OpenText(OptimalFile))
                {
                    return int.Parse(reader.ReadLine());
                }
            }
            catch (FileNotFoundException)
            {
                return defaultValue;
            }
        }

        public void Run()
        {
           
            while (true)
            {
                if (IsGoalReached())
                {
                    break;
                }

                Step();

                Thread.Sleep(200);
            }
        }

        private void Step()
        {
            Beliefs = RoomSensor.Observe(Environment);
            Performance = PerformanceSensor.Observe(Environment);

            if (ActionDone > OptimalSequenceLength)
            {
                Intents.Clear();
                ActionDone = 0;
                return;
            }

            if (Intents.Any())
            {
                var intent = Intents.Dequeue();
                var effector = Effectors[intent];
                effector.DoIt();
                ActionDone++;
            }
            else
            {
                ActionDone = 0;
                PlanIntents(Beliefs);
            }
        }

        public int Learn()
        {
            var oldPerf = Performance;
            do
            {
                Step();
            } while (Intents.Any());

            var newPerf = PerformanceSensor.Observe(Environment);

            return newPerf - oldPerf;
        }
        
        private void PlanIntents(Map actual)
        {
            var state = new State(actual, Action.Idle);
            var tree = new Tree(new Tree.Node(state));
            var strategy = SetStrategy(tree);
            Tree.Node node = null;
            while (strategy.HasNext())
            {
                node = strategy.GetNext();

                if (IsGoalNode(node, strategy) || node.Depth == MaxDepth)
                {
                    break;
                }

                strategy.Expand();
            }

            BacktrackAndBuildIntents(node);
        }

        private Iterator<Tree.Node> SetStrategy(Tree tree)
        {
            if (IsInformed)
            {
                return new AStarIterator(tree);
            }
            
            return new BFSIterator(tree);
        }

        private void BacktrackAndBuildIntents(Tree.Node node)
        {
            if (node.Parent == null)
            {
                return;
            }

            BacktrackAndBuildIntents(node.Parent);
            EnqueueIntentFromNode(node);
        }

        private void EnqueueIntentFromNode(Tree.Node node)
        {
            var intent = node.State.Action;
            Intents.Enqueue(intent);
        }

        private bool IsGoalNode(Tree.Node node, Iterator<Tree.Node> strategy) 
        {
            if (strategy.GetType() == typeof(AStarIterator))
            {
                return false;
            }

            if (node.Parent == null)
            {
                return false;
            }

            var oldJewelCounter = node.Parent.State.Map.PickedJewelCounter;
            var oldDirtCounter = node.Parent.State.Map.SnortedDirtCounter;
            var oldJewelSnortedCounter = node.Parent.State.Map.SnortedJewelCounter;

            var newJewelCounter = node.State.Map.PickedJewelCounter;
            var newDirtCounter = node.State.Map.SnortedDirtCounter;
            var newJewelSnortedCounter = node.State.Map.SnortedJewelCounter;
    
            bool noJewelSnorted = Math.Abs(newJewelSnortedCounter - oldJewelSnortedCounter) < 0.0001;
            bool goal1 = oldJewelCounter < newJewelCounter && noJewelSnorted;
            bool goal2 = oldDirtCounter < newDirtCounter && noJewelSnorted;
            return goal1 || goal2;
        }

        bool IsGoalReached()
        {
            return false;
        }
    }
}