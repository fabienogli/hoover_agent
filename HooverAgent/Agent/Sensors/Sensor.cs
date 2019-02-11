using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public interface Sensor<T>
    {
        T Observe(Mansion env);
    }
}