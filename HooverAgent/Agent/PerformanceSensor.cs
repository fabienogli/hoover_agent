using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class PerformanceSensor : Sensor<int>
    {
        public int Observe(Mansion env)
        {
            return env.Fitness;
        }
    }
}