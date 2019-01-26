using HooverAgent.Agent;
using HooverAgent.Environment;
using Action = System.Action;

namespace HooverAgent
{
    public class Effector
    {
        Feedback intent(Mansion environment, Action action)
        {
            return environment.Try(action);
        } 
    }
}