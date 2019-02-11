using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class LeftEffector : Effector
    {
        public LeftEffector(Mansion mansion) : base(mansion)
        {
        }

        public override bool DoIt()
        {
            return Mansion.HandleMovement(Action.Left);
        }
    }
}