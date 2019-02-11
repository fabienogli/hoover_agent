using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class PickEffector : Effector
    {
        public PickEffector(Mansion mansion) : base(mansion)
        {
        }

        public override bool DoIt()
        {
            return Mansion.HandlePick();
        }
    }
}