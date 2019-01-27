
using System.Threading;
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
            viewer.Subscribe(mansion);
            ThreadStart viewStarter = viewer.Run;
            new Thread(viewStarter).Start();
            
           
            ThreadStart envStarter = mansion.Run;
            new Thread(envStarter).Start();
        }
    }
}