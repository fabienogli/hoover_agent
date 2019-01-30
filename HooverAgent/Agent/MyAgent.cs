using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HooverAgent.Environment;
using HooverAgent.Search;
using Action = HooverAgent.Environment.Action;

namespace HooverAgent.Agent
{
    public class MyAgent
    {
        private RoomSensor RoomSensor { get; }
        private Effector Effector { get; }

        private Mansion Environment { get; }

        private Map Beliefs { get; set; }
        private Queue<Action> Intents { get; }


        public MyAgent(Mansion environment)
        {
            Environment = environment;
            RoomSensor = new RoomSensor();
            Effector = new Effector();
            Intents = new Queue<Action>();
        }

        public void Run()
        {
            while (true)
            {
                Beliefs = RoomSensor.observe(Environment);
                //@TODO know either we stock all the past or not

                if (IsGoalReached())
                {
                    break;
                }

                if (Intents.Any())
                {
                    Action intent = Intents.Dequeue();
                    Effector.DoIt(intent, Environment);
                }
                else
                {
                    PlanIntents(Beliefs);
                }

                Thread.Sleep(50);
            }
        }

        private void PlanIntents(Map actual)
        {
            var state = new State(actual, Action.Idle);
            var tree = new Tree(new Tree.Node(state));
            var strategy = new BFSIterator(tree);
            Tree.Node node = null;
            while (strategy.HasNext())
            {
                node = strategy.GetNext();

                if (IsGoalNode(node))
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
            //todo implement goal nodes !
            return node.State.Map.ContainsEntityAtPos(Entity.Agent, 0);
            //return false;
        }

        bool IsGoalReached()
        {
            return false;
        }
    }
}