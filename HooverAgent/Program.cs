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
            Console.WriteLine("Welcome to the HooverAgent simulation");
            Console.WriteLine("Which mode would you like to run ?");
            Console.WriteLine("1) Normal");
            Console.WriteLine("2) Learn (will update the optimal value for the other runs)");
            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 2)
            {
                Learn();
            }
            else
            {
                Run();
            }
        }

        static void Run()
        {
            Mansion mansion = new Mansion(100);
            Viewer viewer = new Viewer(true);
            VacuumAgent agent = new VacuumAgent(mansion);
            viewer.Subscribe(mansion);

            ThreadStart viewStarter = viewer.Run;
            var viewThread = new Thread(viewStarter);
            viewThread.Start();

            ThreadStart envStarter = mansion.Run;
            var envThread = new Thread(envStarter);
            envThread.Start();

            ThreadStart agentStarter = agent.Run;
            var agentThread = new Thread(agentStarter);
            agentThread.Start();
        }

        static void Learn()
        {
            Console.WriteLine("Learning started");

            const int maxDepth = 6;
            const int learnSteps = 50;

            var information = new Information();
            
            for (var currentDepth = 1; currentDepth < maxDepth; currentDepth++)
            {
                
                Console.WriteLine("Learning for depth " + currentDepth);

                var mansion = new Mansion(100);
                var viewer = new Viewer(false);
                var agent = new VacuumAgent(mansion) {OptimalSequenceLength = currentDepth};
                viewer.Subscribe(mansion);

                var viewThread = new Thread(viewer.Run);
                var envThread = new Thread(mansion.Run);

                viewThread.Start();
                envThread.Start();


                for (var currentIter = 0; currentIter < learnSteps; currentIter++)
                {
                    var perf = agent.Learn();
                    if (perf == 0)
                    {
                        //If the perf is 0, it means the agent has not executed any action but has planned
                        //we don't want to add this value because it will fuck the results up. 
                        continue;
                    }

                    information.AddPerf(currentDepth, perf);
                }
            }

            Console.WriteLine(information.ToString());
            Information.Save(information);
        }
    }
}