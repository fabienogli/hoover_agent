using System;

namespace HooverAgent.Environment
{
    [Flags]
    public enum Entity
    {
        Nothing = 1 << 0,
        Dirt = 1 << 1,
        Jewel = 1 << 2,
        Agent = 1 << 3
    }

    public class EntityStringer
    {
        public static string ObjectToString(Entity obj)
        {
            switch (Entity.Nothing | obj)
            {
                case Entity.Nothing | Entity.Dirt:
                    return "d";
                case Entity.Nothing | Entity.Jewel:
                    return "j";
                case Entity.Nothing:
                    return "-";
                case Entity.Nothing | Entity.Agent:
                    return "x";
                case Entity.Nothing | Entity.Dirt | Entity.Agent:
                    return "0";
                case Entity.Nothing | Entity.Jewel | Entity.Agent:
                    return "1";
                case Entity.Nothing | Entity.Dirt | Entity.Jewel:
                    return "b";
                case Entity.Nothing | Entity.Dirt | Entity.Jewel | Entity.Agent:
                    return "a";
                default:
                    return "-";
            }
        }
    }
}