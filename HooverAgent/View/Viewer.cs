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
        private readonly Queue<Mansion> epochs;
        private Mansion currentEpoch;

        private bool Running { get; set; }

        private bool Display { get; }

        public Viewer(bool display)
        {
            Display = display;
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
                    //Reset timespans
                    n = t;
                    Render();
                }
            }
        }

        private void Render()
        {
            //For learning phase, we don't want to display, but we keep the Notify calls in case we want to have some logs
            if (!Display)
            {
                return;
            }
            Console.Clear();
            GetNextEpoch();
            RenderLegend();
            RenderMap();
            RenderPerf();
        }

        private void RenderPerf()
        {
            Console.WriteLine("Fitness : " + currentEpoch.Fitness);
            Console.WriteLine("Dust snorted : " + currentEpoch.Map.SnortedDirtCounter);
            Console.WriteLine("Jewel picked : " + currentEpoch.Map.PickedJewelCounter);
            Console.WriteLine("Jewel snorted : " + currentEpoch.Map.SnortedJewelCounter);
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
            string agent = EntityStringer.ObjectToString(Entity.Agent);
            string dirt = EntityStringer.ObjectToString(Entity.Dirt);
            string jewel = EntityStringer.ObjectToString(Entity.Jewel);
            string all = EntityStringer.ObjectToString(Entity.Dirt | Entity.Jewel | Entity.Agent);

            string dirtAgent = EntityStringer.ObjectToString(Entity.Agent | Entity.Dirt);
            string jewelAgent = EntityStringer.ObjectToString(Entity.Agent | Entity.Jewel);
            string dirtJewel = EntityStringer.ObjectToString(Entity.Dirt | Entity.Jewel);
            string empty = EntityStringer.ObjectToString(Entity.Nothing);
            string legend =
                $"{agent}=agent {dirt}=dirt {jewel}=jewel {all}=all {dirtAgent}=dirt+agent {jewelAgent}=jewel+agent {dirtJewel}=dirt+jewel {empty}=empty";
            Console.WriteLine(legend);
        }

        private void RenderMap()
        {
            //Assuming the map is squared
            int size = (int) Math.Sqrt(currentEpoch.Map.Size);
            StringBuilder sb = new StringBuilder();
            for (int col = 0; col < size; col++)
            {
                for (int row = 0; row < size; row++)
                {
                    Entity obj = currentEpoch.Map.GetEntityAt(Convert2DTo1D(row, col));
                    string objectString = EntityStringer.ObjectToString(obj);
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


        private int Convert2DTo1D(int col, int row)
        {
            var rowLength = currentEpoch.Map.SquaredSize;

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