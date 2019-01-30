using System;

namespace HooverAgent.Environment
{
    [Flags]
    public enum Entities
    {
        Nothing = 1 << 0,
        Dirt = 1 << 1,
        Jewel = 1 << 2,
        Agent = 1 << 3,
    }

    public class EntitiesStringer
    {
        public static string ObjectToString(Entities obj)
        {
            switch (Entities.Nothing | obj)
            {
                case Entities.Nothing | Entities.Dirt:
                    return "d";
                case Entities.Nothing | Entities.Jewel:
                    return "j";
                case Entities.Nothing:
                    return "-";
                case Entities.Nothing | Entities.Agent:
                    return "x";
                case Entities.Nothing | Entities.Dirt | Entities.Agent:
                    return "0";
                case Entities.Nothing | Entities.Jewel | Entities.Agent:
                    return "1";
                case Entities.Nothing | Entities.Dirt | Entities.Jewel:
                    return "b";
                case Entities.Nothing | Entities.Dirt | Entities.Jewel | Entities.Agent:
                    return "a";
                default:
                    return "-";
            }
        }
    }
}