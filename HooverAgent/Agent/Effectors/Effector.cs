using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public abstract class Effector
    
    {
        protected Mansion Mansion { get;}

        protected Effector(Mansion mansion)
        {
            Mansion = mansion;
        }

        public abstract bool DoIt();

    }
}