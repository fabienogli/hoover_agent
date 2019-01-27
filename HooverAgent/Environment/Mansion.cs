using System;
using System.Collections.Generic;
using System.Threading;

namespace HooverAgent.Environment
{
    public class Mansion : IObservable<Mansion>
    {
        private readonly List<Object> _rooms;
        private Random _rand;
        private IObserver<Mansion> _observer;

        public Mansion(int size)
        {
            _rooms = new List<Object>(size);
            for (var i = 0; i < size; i++)
            {
                _rooms.Add(Object.Nothing);
                //Generate jewels and dirt on first run 
            }

            Running = true;
            
            //Initializing Random once to get uniform result, and seeding to control randomness
            Rand = new Random(1);
        }

        private bool Running { get; }

        private Random Rand { get; }

        public List<Object> Rooms => _rooms;

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

            if ((el & obj) != 0)
            {
                return;
            }

            Rooms[randomIndex] |= obj;
            
            _observer.OnNext(this);
        }

        private bool ShouldGenerateJewel()
        {
            //If some proba
            return false;
        }

        public List<Object> GetRoomsCopy()
        {
           List<Object> roomCopy = new List<Object>(Rooms.Count);
           foreach (var room in Rooms)
           {
               roomCopy.Add(room);
           }

           return roomCopy;
        }

        public IDisposable Subscribe(IObserver<Mansion> observer)
        {
            _observer = observer;
            _observer.OnNext(this);
            return null; //Maybe return an Unsubscriber
        }
    }
}


