using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public interface Sensor<T>
    {
        T observe(Mansion envi);
    }
}