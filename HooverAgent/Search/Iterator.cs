namespace HooverAgent.Search
{
    public interface Iterator<T>
    {
        bool HasNext();
        T GetNext();
    }
}