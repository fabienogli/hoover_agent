using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class SnortEffector : Effector
    {
        public SnortEffector(Mansion mansion) : base(mansion)
        {
        }

        public override bool DoIt()
        {
            return Mansion.HandleSnort();
        }
    }
}