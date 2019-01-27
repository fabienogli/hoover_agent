using System;

namespace HooverAgent.Environment
{
    [Flags]
    public enum Object
    {
        Nothing = 1 << 0,
        Dirt = 1 << 1,
        Jewel = 1 << 2,
        Agent = 1 << 3,
    }

    public class ObjectStringer
    {
        public static string ObjectToString(Object obj)
        {
            switch (Object.Nothing | obj)
            {
                case Object.Nothing | Object.Dirt:
                    return "d";
                case Object.Nothing | Object.Jewel:
                    return "j";
                case Object.Nothing:
                    return "-";
                case Object.Nothing | Object.Agent:
                    return "x";
                case Object.Nothing | Object.Dirt | Object.Agent:
                    return "0";
                case Object.Nothing | Object.Jewel | Object.Agent:
                    return "1";
                case Object.Nothing | Object.Dirt | Object.Jewel:
                    return "b";
                case Object.Nothing | Object.Dirt | Object.Jewel | Object.Agent:
                    return "a";
                default:
                    return "-";
            }
        }
    }
}