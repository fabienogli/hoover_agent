using System;
using System.Collections.Generic;
using System.Threading;

namespace HooverAgent.Environment
{
    public class Mansion : IObservable<Mansion>
    {
        private Random _rand;
        private IObserver<Mansion> _observer;

        public Mansion(int size)
        {
            Rooms = new List<Object>(size);
            for (var i = 0; i < size; i++)
            {
                Rooms.Add(Object.Nothing);
                //Generate jewels and dirt on first run 
            }

            Running = true;
            
            //Initializing Random once to get uniform result, and seeding to control randomness
            Rand = new Random(1);
        }

        public Mansion(Mansion other)
        {
            Rooms = new List<Object>(other.Rooms.Count);
            foreach (var room in other.Rooms)
            {
                Rooms.Add(room);
            }
            //Copy agent state (pos) 
            //Copy performance 
        }

        private bool Running { get; }

        private Random Rand { get; }

        public List<Object> Rooms { get; }

        public void Run()
        {
            
            while (Running)
            {
                if (ShouldGenerateDirt())
                {
                    GenerateDirt();
                }

                if (ShouldGenerateJewel())
                {
                    GenerateJewel();
                }
                Thread.Sleep(10);
            }
        }

        private bool ShouldGenerateDirt()
        {
            //If some proba
            //@todo : Generate with timer => Use StopWatch i.e : Every n seconds, there is a probability p of generating dirt
            //todo : Verify dirt distribution (should not fill all map)

            return Rand.Next(100) < 5;
        }

        private bool ShouldGenerateJewel()
        {
            //If some proba
            return Rand.Next(100) < 5;
        }
        
        private void GenerateDirt()
        {
            GenerateObject(Object.Dirt);
        }
        
        private void GenerateJewel()
        {
            GenerateObject(Object.Jewel);
        }
        
        private void GenerateObject(Object obj)
        {
            int randomIndex = Rand.Next(Rooms.Count);
            Object el = Rooms[randomIndex];

            if (el.HasFlag(obj))
            {
                //Already set so no need to reset it
                return;
            }

            Rooms[randomIndex] |= obj;
            _observer.OnNext(this);
        }
        public IDisposable Subscribe(IObserver<Mansion> observer)
        {
            _observer = observer;
            _observer.OnNext(this);
            return null; //Maybe return an Unsubscriber
        }
    }
}


