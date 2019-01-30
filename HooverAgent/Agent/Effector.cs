using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class Effector
    {
        public bool DoIt(Action action, Mansion mansion)
        {
            return mansion.HandleRequest(action);
        }
    }
}