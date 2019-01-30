
using System;
using System.Collections.Generic;
using System.Threading;
using HooverAgent.Agent;
using HooverAgent.Environment;
using HooverAgent.View;

namespace HooverAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Mansion mansion = new Mansion(100);
            Viewer viewer = new Viewer();
            MyAgent agent = new MyAgent(mansion);
            viewer.Subscribe(mansion);
            ThreadStart viewStarter = viewer.Run;
            new Thread(viewStarter).Start();
            
           
            ThreadStart envStarter = mansion.Run;
            new Thread(envStarter).Start();

            ThreadStart agentStarter = agent.Run;
            new Thread(agentStarter).Start();

        }

        static void test(Queue<int> queue)
        {
            queue.Enqueue(1);
        }
    }
}