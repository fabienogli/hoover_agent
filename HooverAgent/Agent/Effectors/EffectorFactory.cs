
using System.Collections.Generic;
using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class EffectorFactory
    {
        public static Dictionary<Action, Effector> GetActionEffectorDictionary(Mansion mansion)
        {
            Dictionary<Action, Effector> dict = new Dictionary<Action, Effector>();
            dict.Add(Action.Down, new DownEffector(mansion));
            dict.Add(Action.Up, new UpEffector(mansion));
            dict.Add(Action.Right, new RightEffector(mansion));
            dict.Add(Action.Left, new LeftEffector(mansion));
            dict.Add(Action.Pick, new PickEffector(mansion));
            dict.Add(Action.Snort, new SnortEffector(mansion));

            return dict;
        }
    }
}