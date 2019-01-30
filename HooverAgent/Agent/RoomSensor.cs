using System.Collections.Generic;
using HooverAgent.Environment;

namespace HooverAgent.Agent
{
    public class RoomSensor : Sensor<List<Entities>>
    {
        public List<Entities> observe(Mansion env)
        {
            return env.CopyRooms();
        }
    }
}