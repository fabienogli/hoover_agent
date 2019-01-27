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
}