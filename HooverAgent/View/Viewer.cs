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
        private Queue<Mansion> epochs;
        private Mansion currentEpoch;

        private bool Running { get; set; }

        public Viewer()
        {
            Running = true;
            epochs = new Queue<Mansion>();
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
            Console.Clear();
            GetNextEpoch();
            RenderLegend();
            RenderMap();
        }

        private void GetNextEpoch()
        {
            if (epochs.Count > 0)
            {
                currentEpoch = epochs.Dequeue();
            }
        }

        private void RenderLegend()
        {
            string agent = ObjectStringer.ObjectToString(Object.Agent);
            string dirt = ObjectStringer.ObjectToString(Object.Dirt);
            string jewel = ObjectStringer.ObjectToString(Object.Jewel);
            string all = ObjectStringer.ObjectToString(Object.Dirt | Object.Jewel | Object.Agent);
            
            string dirtAgent = ObjectStringer.ObjectToString(Object.Agent | Object.Dirt);
            string jewelAgent = ObjectStringer.ObjectToString(Object.Agent | Object.Jewel);
            string dirtJewel = ObjectStringer.ObjectToString(Object.Dirt | Object.Jewel);
            string empty = ObjectStringer.ObjectToString(Object.Nothing);
            string legend = $"{agent}=agent {dirt}=dirt {jewel}=jewel {all}=all {dirtAgent}=dirt+agent {jewelAgent}=jewel+agent {dirtJewel}=dirt+jewel {empty}=empty";
            Console.WriteLine(legend);
        }

        private void RenderMap()
        {
            //Assuming the map is squared
            int size = (int) Math.Sqrt(currentEpoch.Rooms.Count);

            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    Object obj = currentEpoch.Rooms[Convert2dTo1d(col, row)];
                    string objectString = ObjectStringer.ObjectToString(obj);
                    if (row == 0)
                    {
                        Console.Write("| " + objectString + " | ");
                    }
                    else
                    {
                        Console.Write(objectString + " | ");
                    }
                }

                Console.WriteLine();
            }
        }


        public int Convert2dTo1d(int col, int row)
        {
            int rowLength = (int) Math.Sqrt(currentEpoch.Rooms.Count);
            return row * rowLength + col;
        }

        public virtual void Subscribe(IObservable<Mansion> observable)
        {
            observable.Subscribe(this);
            currentEpoch = (Mansion) observable;
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
            Mansion copy = new Mansion(mansion);
            epochs.Enqueue(copy);
        }
    }
}