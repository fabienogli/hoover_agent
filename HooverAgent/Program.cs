using System;
using System.Threading;
using HooverAgent.Environment;

namespace HooverAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Mansion mansion = new Mansion(100);
            ThreadStart envStarter = mansion.Run;
            new Thread(envStarter).Start();
            
        }
    }
}