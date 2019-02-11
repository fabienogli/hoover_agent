using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class RightEffector : Effector
    {
        public RightEffector(Mansion mansion) : base(mansion)
        {
        }

        public override bool DoIt()
        {
            return Mansion.HandleMovement(Action.Right);
        }
    }
}