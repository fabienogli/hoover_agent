namespace HooverAgent.Agent
{
    public interface Sensor<T>
    {
        T observe();
    }
}