using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class DownEffector : Effector
    {
        public DownEffector(Mansion mansion) : base(mansion)
        {
        }

        public override bool DoIt()
        {
            return Mansion.HandleMovement(Action.Down);
        }
    }
}