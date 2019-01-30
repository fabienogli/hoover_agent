using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HooverAgent.Environment;
using HooverAgent.Search;

namespace HooverAgent.Agent
{
    public class MyAgent
    {
        private RoomSensor RoomSensor;
        private Effector Effector;
        private readonly Mansion Environment;
        private List<Entities> Beliefs;
        private Queue<Action> Intents;


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
                    Effector.Dewit(intent, Environment);
                }
                else
                {
                    PlanIntents(Beliefs);
                }
                Thread.Sleep(50);
            }
        }

        private void PlanIntents(List<Entities> actual)
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
                //EnqueueIntentFromNode(node);
                return;
            }

            BacktrackAndBuildIntents(node.Parent);
            EnqueueIntentFromNode(node);
        }

        private void EnqueueIntentFromNode(Tree.Node node)
        {
            var intent = node.State.GetAction();
            Intents.Enqueue(intent);
        }

        private bool IsGoalNode(Tree.Node node)
        {
            //todo implement goal nodes !
            return node.State.GetMap()[0].HasFlag(Entities.Agent);
            //return false;
        }

        bool IsGoalReached()
        {
            return false;
        }
    }
}