using System.Collections.Generic;

namespace HooverAgent.Agent
{
    public class Agent
    {
        private List<Effector> _effectors;
        private List<Captor> _captors;

        public List<Effector> Effectors
        {
            get => _effectors;
            set => _effectors = value;
        }

        public List<Captor> Captors
        {
            get => _captors;
            set => _captors = value;
        }
    }
}