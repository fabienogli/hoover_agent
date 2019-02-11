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
            Thread envThread = null;
            Thread viewThread = null;

            const int maxDepth = 6;
            const int learnSteps = 50;

            Information information = new Information();
            
            for (var currentDepth = 1; currentDepth < maxDepth; currentDepth++)
            {
                envThread?.Abort();
                viewThread?.Abort();

                var mansion = new Mansion(100);
                var viewer = new Viewer(false);
                var agent = new VacuumAgent(mansion);
                agent.OptimalSequenceLength = currentDepth;
                viewer.Subscribe(mansion);

                viewThread = new Thread(viewer.Run);
                envThread = new Thread(mansion.Run);

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
           
        }
    }
}