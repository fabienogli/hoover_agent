using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class UpEffector : Effector
    {
        public UpEffector(Mansion mansion) : base(mansion)
        {
        }

        public override bool DoIt()
        {
            return Mansion.HandleMovement(Action.Up);
        }
    }
}