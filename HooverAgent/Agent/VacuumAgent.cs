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
        private const int MaxDepth = 5;
        private RoomSensor RoomSensor { get; }
        private PerformanceSensor PerformanceSensor { get; }
        private Effector Effector { get; }

        private Mansion Environment { get; }

        private Map Beliefs { get; set; }

        private int Performance { get; set; }
        private Queue<Action> Intents { get; }

        public int OptimalSequenceLength { private get; set; }

        private int ActionDone { get; set; }

        public VacuumAgent(Mansion environment)
        {
            Environment = environment;
            RoomSensor = new RoomSensor();
            PerformanceSensor = new PerformanceSensor();
            Effector = new Effector();
            Intents = new Queue<Action>();
            OptimalSequenceLength = ReadOptimalFromFileOrDefault(MaxDepth);
            ActionDone = 0;
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
                Effector.DoIt(intent, Environment);
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

            return Performance - oldPerf;
        }

        private void PlanIntents(Map actual)
        {
            var state = new State(actual, Action.Idle);
            var tree = new Tree(new Tree.Node(state));
            var strategy = new AStarIterator(tree);
            Tree.Node node = null;
            while (strategy.HasNext())
            {
                node = strategy.GetNext();

                if (IsGoalNode(node) || node.Depth == MaxDepth)
                {
                    break;
                }

                strategy.Expand();
            }

            BacktrackAndBuildIntents(node);
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

        private bool IsGoalNode(Tree.Node node)
        {
            return false;
        }

        bool IsGoalReached()
        {
            return false;
        }
    }
}