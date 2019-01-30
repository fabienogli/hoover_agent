using System.Collections.Generic;
using System.Linq;
using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class Agent
    {
        private RoomSensor RoomSensor;
        private Effector Effector;
        private Mansion Environment;
        private List<Entities> Beliefs;
        private List<Action> Intents;
        

        public Agent(Mansion environment)
        {
            Environment = environment;
            Intents = new List<Action>();
        }
        public void run()
        {
            Beliefs = RoomSensor.observe(Environment);
            //@TODO know either we stock all the past or not
            if (!IsGoalReached())
            {
                if (Intents.Any())
                {
                    Action action = Intents.First();
                    Intents.RemoveAt(0);
                    Effector.Dewit(action, Environment);
                }
                else
                {
                    Intents = ProcessObservations(Beliefs);
                }
            }
            
        }

        private List<Action> ProcessObservations(List<Entities> actual)
        {
            throw new System.NotImplementedException();
        }

        bool IsGoalReached()
        {
            return false;
        }
    }
}