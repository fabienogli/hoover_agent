using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using HooverAgent.Environment;
using Object = HooverAgent.Environment.Object;

namespace HooverAgent.View
{
    public class Viewer : IObserver<Mansion>
    {

        private IDisposable _canceller;

        public bool Running { get; set; }

        public Viewer()
        {
            Running = true;
        }

        public void Run()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            TimeSpan n = watch.Elapsed;
            Render();
            while (Running)
            {
                TimeSpan t = watch.Elapsed;
                if (t.TotalMilliseconds - n.TotalMilliseconds > 800)
                {
                    n = t;
                    //Render here
                    Render();
                }
               
            }
        }

        private void Render()
        {
            //Render here
            // Poll mansion copy from queue, if empty, running = false
            // Display mansion rooms
            // Display agent at pos
            // Display perf
            
        }

        public virtual void Subscribe(IObservable<Mansion> observable)
        {
            _canceller = observable.Subscribe(this);
        }

        public void OnCompleted()
        {
            //End 
            Running = false;
        }

        public void OnError(Exception error)
        {
           /*
            * Does nothing
            */
        }

        public void OnNext(Mansion mansion)
        {
            List<Object> rooms = mansion.GetRoomsCopy();
           
            //Get performance copy
            //Get Agent position copy
        }
    }
}