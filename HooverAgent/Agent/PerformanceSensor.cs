using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class PerformanceSensor : Sensor<int>
    {
        public int observe(Mansion env)
        {
            return env.Fitness;
        }
    }
}