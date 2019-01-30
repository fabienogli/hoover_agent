using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using HooverAgent.Environment;

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
            string agent = EntitiesStringer.ObjectToString(Entities.Agent);
            string dirt = EntitiesStringer.ObjectToString(Entities.Dirt);
            string jewel = EntitiesStringer.ObjectToString(Entities.Jewel);
            string all = EntitiesStringer.ObjectToString(Entities.Dirt | Entities.Jewel | Entities.Agent);
            
            string dirtAgent = EntitiesStringer.ObjectToString(Entities.Agent | Entities.Dirt);
            string jewelAgent = EntitiesStringer.ObjectToString(Entities.Agent | Entities.Jewel);
            string dirtJewel = EntitiesStringer.ObjectToString(Entities.Dirt | Entities.Jewel);
            string empty = EntitiesStringer.ObjectToString(Entities.Nothing);
            string legend = $"{agent}=agent {dirt}=dirt {jewel}=jewel {all}=all {dirtAgent}=dirt+agent {jewelAgent}=jewel+agent {dirtJewel}=dirt+jewel {empty}=empty";
            Console.WriteLine(legend);
        }

        private void RenderMap()
        {
            //Assuming the map is squared
            int size = (int) Math.Sqrt(currentEpoch.Rooms.Count);
            StringBuilder sb = new StringBuilder();
            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    Entities obj = currentEpoch.Rooms[Convert2dTo1d(col, row)];
                    string objectString = EntitiesStringer.ObjectToString(obj);
                    if (row == 0)
                    {
                        sb.Append("| ");
                    }

                    sb.Append(objectString)
                      .Append(" | ");
                }

                sb.Append(System.Environment.NewLine);
            }
            Console.Write(sb.ToString());
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