using System.Collections.Generic;
using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class RoomSensor : Sensor<Map>
    {
        public Map observe(Mansion env)
        {
            return new Map(env.Map);
        }
    }
}